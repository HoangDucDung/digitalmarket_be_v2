pipeline {
    agent any

    stages {
        stage('Clear Workspace') {
            steps {
                cleanWs()
            }
        }

        stage('Checkout Code') {
            steps {
               echo '✅ Đang kéo source code từ Github...'
               checkout scm
            }
        }

        stage('Docker Build (.NET App)') {
            steps {
                echo '✅ Triển khai docker-in-docker: Gọi Docker Engine để biên dịch và đóng gói...'
                // Bắt Docker build file Image dựa trên Dockerfile của dự án .NET
                sh 'docker build -t digitalmarket-be:latest .'
            }
        }

        stage('Deploy/Run Container') {
            steps {
                echo '✅ Khởi chạy phiên bản mới...'
                sh '''
                    # Tắt và rọn dẹp bản cũ nếu đang chạy (nếu không có thì bỏ qua lỗi)
                    docker stop digitalmarket-container || true
                    docker rm digitalmarket-container || true
                    
                    # Chạy Image mới vừa build ở port 8080. (Lứu ý: thay 8080 bằng port ứng dụng thực tế bạn dùng)
                    docker run -d -p 8080:8080 --name digitalmarket-container digitalmarket-be:latest
                '''
            }
        }
    }

    post {
        success {
            echo "🎉 Pipeline hoàn tất xuất sắc! Docker container đang chạy phiên bản mới nhất."
        }
        failure {
            echo "❌ Pipeline thất bại, vui lòng đọc Console Output ở trên để tìm lỗi."
        }
    }
}
