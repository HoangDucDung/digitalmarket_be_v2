pipeline {
    agent any

    environment {
        // ── Tên Docker image ──
        IMAGE_AUTH     = 'digitalmarket-auth'
        IMAGE_BUSINESS = 'digitalmarket-business'

        // ── Tên container ──
        CONTAINER_AUTH     = 'digitalmarket-auth-container'
        CONTAINER_BUSINESS = 'digitalmarket-business-container'

        // ── Đường dẫn Dockerfile từng microservice ──
        DOCKERFILE_AUTH     = 'Presentation/API/Digitalmarket.Controller.Auth/Dockerfile'
        DOCKERFILE_BUSINESS = 'Presentation/API/Digitalmarket.Controller.Business/Dockerfile'

        // ── Port mapping (host:container) ──
        PORT_AUTH     = '8080'
        PORT_BUSINESS = '8081'

        // ── Tag image theo số build của Jenkins ──
        IMAGE_TAG = "${env.BUILD_NUMBER}"
    }

    stages {
        stage('CI: Restore & Build') {
            steps {
                cleanWs() // Xoá sạch workspace cũ
                checkout scm // Kéo code từ SCM (Github/Gitlab) về
                
                // Di chuyển githubNotify xuống SAU KHI checkout scm để Jenkins có đủ thông tin git (commit sha, repo)
                githubNotify(status: 'PENDING', context: "${env.GITHUB_STATUS_CONTEXT ?: 'Jenkins CI/CD'}", description: 'Đang kiểm tra code...')
                
                echo 'Restoring packages & building...'
                sh 'dotnet restore'
                sh 'dotnet build --no-restore --configuration Release'
            }

            post {
                success {
                    // Chỉ báo SUCCESS sau khi CI xong, không đợi CD
                    githubNotify(
                        status: 'SUCCESS', 
                        context: "${env.GITHUB_STATUS_CONTEXT ?: 'Jenkins CI/CD'} - CI", 
                        description: 'CI passed!')
                }
                failure {
                    githubNotify(
                        status: 'FAILURE', 
                        context: "${env.GITHUB_STATUS_CONTEXT ?: 'Jenkins CI/CD'} - CI", 
                        description: 'CI failed!')
                }
            }
        }

        stage('CD: Docker Build & Deploy Microservices') {
            when {
                branch 'main'
            }
            stages {
                // ──────────────────────────────────────────────
                // 1. Chạy Unit Tests (Bỏ comment nếu cần kích hoạt)
                // ──────────────────────────────────────────────
                // stage('Run Tests') {
                //     steps {
                //         echo '🧪 Đang chạy Unit Tests...'
                //         sh 'dotnet test Digitalmarket.sln --logger "trx;LogFileName=test_results.trx"'
                //     }
                // }

                // ──────────────────────────────────────────────
                // 2. Build Docker images song song (Microservices)
                // ──────────────────────────────────────────────
                stage('Docker Build - Microservices') {
                    parallel {
                        stage('Build Auth API') {
                            steps {
                                echo "🔨 Building ${IMAGE_AUTH}:${IMAGE_TAG} từ ${DOCKERFILE_AUTH}"
                                sh """
                                    docker build \
                                        -f ${DOCKERFILE_AUTH} \
                                        -t ${IMAGE_AUTH}:${IMAGE_TAG} \
                                        -t ${IMAGE_AUTH}:latest \
                                        .
                                """
                            }
                        }
                        stage('Build Business API') {
                            steps {
                                echo "🔨 Building ${IMAGE_BUSINESS}:${IMAGE_TAG} từ ${DOCKERFILE_BUSINESS}"
                                sh """
                                    docker build \
                                        -f ${DOCKERFILE_BUSINESS} \
                                        -t ${IMAGE_BUSINESS}:${IMAGE_TAG} \
                                        -t ${IMAGE_BUSINESS}:latest \
                                        .
                                """
                            }
                        }
                    }
                }

                // ──────────────────────────────────────────────
                // 3. Dừng & xoá container cũ (nếu có)
                // ──────────────────────────────────────────────
                stage('Stop Old Containers') {
                    steps {
                        echo '🛑 Dừng và xoá container phiên bản cũ...'
                        sh """
                            docker stop ${CONTAINER_AUTH} || true
                            docker rm   ${CONTAINER_AUTH} || true

                            docker stop ${CONTAINER_BUSINESS} || true
                            docker rm   ${CONTAINER_BUSINESS} || true
                        """
                    }
                }

                // ──────────────────────────────────────────────
                // 4. Deploy từng microservice
                // ──────────────────────────────────────────────
                stage('Deploy Containers') {
                    steps {
                        echo '🚀 Khởi chạy các microservice...'
                        sh """
                            docker run -d \
                                --name ${CONTAINER_AUTH} \
                                -p ${PORT_AUTH}:8080 \
                                --restart unless-stopped \
                                ${IMAGE_AUTH}:${IMAGE_TAG}

                            docker run -d \
                                --name ${CONTAINER_BUSINESS} \
                                -p ${PORT_BUSINESS}:8080 \
                                --restart unless-stopped \
                                ${IMAGE_BUSINESS}:${IMAGE_TAG}
                        """
                    }
                }

                // ──────────────────────────────────────────────
                // 5. Health Check
                // ──────────────────────────────────────────────
                stage('Health Check') {
                    steps {
                        echo '🏥 Kiểm tra trạng thái containers...'
                        sh """
                            sleep 10

                            echo '--- Auth API ---'
                            docker ps --filter "name=${CONTAINER_AUTH}" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

                            echo '--- Business API ---'
                            docker ps --filter "name=${CONTAINER_BUSINESS}" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

                            AUTH_STATUS=\$(docker inspect -f '{{.State.Running}}' ${CONTAINER_AUTH} 2>/dev/null || echo "false")
                            BIZ_STATUS=\$(docker inspect -f '{{.State.Running}}' ${CONTAINER_BUSINESS} 2>/dev/null || echo "false")

                            if [ "\$AUTH_STATUS" != "true" ]; then
                                echo "❌ Auth API container không chạy được!"
                                docker logs ${CONTAINER_AUTH} --tail 30 || true
                                exit 1
                            fi

                            if [ "\$BIZ_STATUS" != "true" ]; then
                                echo "❌ Business API container không chạy được!"
                                docker logs ${CONTAINER_BUSINESS} --tail 30 || true
                                exit 1
                            fi

                            echo "✅ Tất cả microservice đang chạy bình thường!"
                        """
                    }
                }

                // ──────────────────────────────────────────────
                // 6. Dọn dẹp Docker images cũ
                // ──────────────────────────────────────────────
                stage('Cleanup Old Images') {
                    steps {
                        echo '🧹 Dọn dẹp Docker images cũ...'
                        sh """
                            docker image prune -f || true
                            docker images ${IMAGE_AUTH} --format '{{.Tag}}' | grep -E '^[0-9]+\$' | sort -rn | tail -n +4 | xargs -r -I{} docker rmi ${IMAGE_AUTH}:{} || true
                            docker images ${IMAGE_BUSINESS} --format '{{.Tag}}' | grep -E '^[0-9]+\$' | sort -rn | tail -n +4 | xargs -r -I{} docker rmi ${IMAGE_BUSINESS}:{} || true
                        """
                    }
                }
            }
        }
    }

    post {
        success {
            githubNotify(
                status: 'SUCCESS',
                context: "${env.GITHUB_STATUS_CONTEXT ?: 'Jenkins CI/CD'} - Deployment",
                description: 'Build & Deploy thành công!'
            )
            echo """
            🎉 ═══════════════════════════════════════════════
               PIPELINE HOÀN TẤT THÀNH CÔNG!
            ═══════════════════════════════════════════════════
               📦 Build:          #${env.BUILD_NUMBER}
            ═══════════════════════════════════════════════════
            """
        }
        failure {
            githubNotify(
                status: 'FAILURE',
                context: "${env.GITHUB_STATUS_CONTEXT ?: 'Jenkins CI/CD'} - Deployment",
                description: 'Build hoặc Deploy thất bại!'
            )
            echo '❌ Pipeline thất bại! Vui lòng kiểm tra Console Output ở trên để tìm lỗi.'
        }
        always {
            cleanWs(notFailBuild: true)
        }
    }
}
