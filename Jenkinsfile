node {
    // ── 1. Khai báo các Biến Môi Trường chung ──
    def IMAGE_AUTH     = 'digitalmarket-auth'
    def IMAGE_BUSINESS = 'digitalmarket-business'
    def CONTAINER_AUTH     = 'digitalmarket-auth-container'
    def CONTAINER_BUSINESS = 'digitalmarket-business-container'
    def DOCKERFILE_AUTH     = 'Presentation/API/Digitalmarket.Controller.Auth/Dockerfile'
    def DOCKERFILE_BUSINESS = 'Presentation/API/Digitalmarket.Controller.Business/Dockerfile'
    def PORT_AUTH     = '8080'
    def PORT_BUSINESS = '8081'
    def IMAGE_TAG     = "${env.BUILD_NUMBER}"

    def GITHUB_CREDENTIAL_ID = 'token-git-v1' // Đổi thành 'token-github' nếu Jenkins báo lỗi Credential
    def GITHUB_ACCOUNT       = 'HoangDucDung'
    def GITHUB_REPO          = 'digitalmarket_be_v2'

    // ── 2. Khởi tạo môi trường cô lập ──
    withEnv(["DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true"]) {
        try {
            cleanWs() // Dọn sạch kho chứa
            checkout scm // Tải mã nguồn mới về

            // 🔥 3. BỘ PHÁT ĐIỀN THÔNG MINH (IF-ELSE THỰC THỤ)
            // Tự động nhận biết đang chạy trên nhánh Main hay nhánh PR
            def isMainBranch = (env.BRANCH_NAME == 'main') || (env.GIT_BRANCH != null && env.GIT_BRANCH.contains('main'))

            // =====================================================
            // 🚀 PHÂN LUỒNG CI (CHỈ VẼ TRÊN GIAO DIỆN CỦA PULL REQUEST!)
            // =====================================================
            
            // Tải công cụ .NET SDK động của Scripted Pipeline
            def dotnetHome = tool name: 'dotnet8', type: 'dotnetsdk'
            
            withEnv(["PATH+DOTNET=${dotnetHome}"]) {
                stage('CI: Restore & Build') {
                    // Báo cáo Đang chạy cho PR
                    githubNotify(
                        status: 'PENDING',
                        context: "Jenkins CI/CD - CI",
                        description: 'Đang kiểm tra compile code...',
                        credentialsId: GITHUB_CREDENTIAL_ID,
                        account: GITHUB_ACCOUNT,
                        repo: GITHUB_REPO,
                        sha: env.GIT_COMMIT
                    )

                    echo '🔨 Đang build kiểm thử Pull Request...'
                    sh 'dotnet restore'
                    sh 'dotnet build --no-restore --configuration Release'

                    // Báo cáo THÀNH CÔNG cho PR
                    githubNotify(
                        status: 'SUCCESS',
                        context: "Jenkins CI/CD - CI",
                        description: 'CI Passed! Sẵn sàng Merge.',
                        credentialsId: GITHUB_CREDENTIAL_ID,
                        account: GITHUB_ACCOUNT,
                        repo: GITHUB_REPO,
                        sha: env.GIT_COMMIT
                    )
                }
            }

            if (isMainBranch) {
                // =====================================================
                // 📦 PHÂN LUỒNG CD (CHỈ VẼ TRÊN GIAO DIỆN JOB MAIN!)
                // =====================================================
                
                stage('CD: Initialize') {
                    githubNotify(
                        status: 'PENDING',
                        context: "Jenkins CI/CD - Deployment",
                        description: 'Bắt đầu quá trình CD...',
                        credentialsId: GITHUB_CREDENTIAL_ID,
                        account: GITHUB_ACCOUNT,
                        repo: GITHUB_REPO,
                        sha: env.GIT_COMMIT
                    )
                }

                stage('CD: Docker Build') {
                    parallel(
                        "Build Auth": {
                            sh """
                                docker build -f ${DOCKERFILE_AUTH} -t ${IMAGE_AUTH}:${IMAGE_TAG} -t ${IMAGE_AUTH}:latest .
                            """
                        },
                        "Build Business": {
                            sh """
                                docker build -f ${DOCKERFILE_BUSINESS} -t ${IMAGE_BUSINESS}:${IMAGE_TAG} -t ${IMAGE_BUSINESS}:latest .
                            """
                        }
                    )
                }

                stage('CD: Deploy') {
                    sh """
                        docker stop ${CONTAINER_AUTH} || true
                        docker rm   ${CONTAINER_AUTH} || true
                        docker stop ${CONTAINER_BUSINESS} || true
                        docker rm   ${CONTAINER_BUSINESS} || true
                        
                        docker run -d --name ${CONTAINER_AUTH} -p ${PORT_AUTH}:8080 --restart unless-stopped ${IMAGE_AUTH}:${IMAGE_TAG}
                        docker run -d --name ${CONTAINER_BUSINESS} -p ${PORT_BUSINESS}:8080 --restart unless-stopped ${IMAGE_BUSINESS}:${IMAGE_TAG}
                    """
                }

                stage('CD: Health Check') {
                    sh """
                        sleep 10
                        AUTH_STATUS=\$(docker inspect -f '{{.State.Running}}' ${CONTAINER_AUTH} 2>/dev/null || echo "false")
                        BIZ_STATUS=\$(docker inspect -f '{{.State.Running}}' ${CONTAINER_BUSINESS} 2>/dev/null || echo "false")
                        if [ "\$AUTH_STATUS" != "true" ] || [ "\$BIZ_STATUS" != "true" ]; then
                            exit 1
                        fi
                    """
                }

                stage('CD: Cleanup') {
                    sh """
                        docker image prune -f || true
                        docker images ${IMAGE_AUTH} --format '{{.Tag}}' | grep -E '^[0-9]+\$' | sort -rn | tail -n +4 | xargs -r -I{} docker rmi ${IMAGE_AUTH}:{} || true
                        docker images ${IMAGE_BUSINESS} --format '{{.Tag}}' | grep -E '^[0-9]+\$' | sort -rn | tail -n +4 | xargs -r -I{} docker rmi ${IMAGE_BUSINESS}:{} || true
                    """
                    // Báo cáo THÀNH CÔNG cho nhánh Deploy
                    githubNotify(
                        status: 'SUCCESS',
                        context: "Jenkins CI/CD - Deployment",
                        description: 'Deploy thành công lên Production!',
                        credentialsId: GITHUB_CREDENTIAL_ID,
                        account: GITHUB_ACCOUNT,
                        repo: GITHUB_REPO,
                        sha: env.GIT_COMMIT
                    )
                }

            }

        } catch (Exception err) {
            // 🚨 Bắt lỗi (Tương tự post failure) và đẩy cảnh báo đỏ lên GitHub
            def isMainBranch = (env.BRANCH_NAME == 'main') || (env.GIT_BRANCH != null && env.GIT_BRANCH.contains('main'))
            
            if (env.GIT_COMMIT) {
                githubNotify(
                    status: 'FAILURE',
                    context: "Jenkins CI/CD - CI",
                    description: 'CI Failed! Lỗi biên dịch code.',
                    credentialsId: GITHUB_CREDENTIAL_ID,
                    account: GITHUB_ACCOUNT,
                    repo: GITHUB_REPO,
                    sha: env.GIT_COMMIT
                )

                if (isMainBranch) {
                    githubNotify(
                        status: 'FAILURE',
                        context: "Jenkins CI/CD - Deployment",
                        description: 'Deploy thất bại! Kiểm tra logs.',
                        credentialsId: GITHUB_CREDENTIAL_ID,
                        account: GITHUB_ACCOUNT,
                        repo: GITHUB_REPO,
                        sha: env.GIT_COMMIT
                    )
                }
            }
            throw err // Ném lỗi ra ngoài để Jenkins đổi màu Job sang ĐỎ
        } finally {
            cleanWs(notFailBuild: true) // Dọn dẹp workspace luôn luôn
        }
    }
}
