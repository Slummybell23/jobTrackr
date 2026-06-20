pipeline {
    agent any

    environment {
        IMAGE_NAME = "jobtrackr"
    }

    stages {
        stage('Pull Git Repo') {
            steps {
                git branch: 'main',
                    credentialsId: '11e57030-73f3-496b-a91c-3773a5a31b08',
                    url: 'http://192.168.86.239:4002/slummybell/caches-job-trackr'
                sh 'pwd'
                sh 'ls'
            }
        }

        stage('Build and Push Docker Image to DockerHub') {
            steps {
                // The multi-stage Dockerfile lives at the repo root and builds
                // both the React web UI and the .NET API, so the build context
                // is the workspace root (".").
                withCredentials([usernamePassword(
                    credentialsId: '40504039-5964-4043-a2f6-999842532b2c',
                    usernameVariable: 'DOCKERHUB_USER',
                    passwordVariable: 'DOCKERHUB_TOKEN'
                    )]) {
                        sh '''
                        echo "$DOCKERHUB_TOKEN" | docker login -u "$DOCKERHUB_USER" --password-stdin

                        docker build -t "$DOCKERHUB_USER/${IMAGE_NAME}:${BUILD_NUMBER}" .

                        docker tag "$DOCKERHUB_USER/${IMAGE_NAME}:${BUILD_NUMBER}" "$DOCKERHUB_USER/${IMAGE_NAME}:latest"
                        docker tag "$DOCKERHUB_USER/${IMAGE_NAME}:${BUILD_NUMBER}" "$DOCKERHUB_USER/${IMAGE_NAME}:latestdev"

                        # Push all three tags
                        docker push "$DOCKERHUB_USER/${IMAGE_NAME}:${BUILD_NUMBER}"
                        docker push "$DOCKERHUB_USER/${IMAGE_NAME}:latest"
                        docker push "$DOCKERHUB_USER/${IMAGE_NAME}:latestdev"

                        docker logout
                      '''
                    }
            }
        }

        stage('Clean Docker Images') {
            steps {
                sh 'docker rmi $(docker images -a -q) || true'
            }
        }

        stage('Deploy via Watchtower to Unraid Server') {
            steps {
                sh '''#!/bin/sh
                set -eu
                curl -fSs -X POST \
                  -H "Authorization: Bearer TRIGGERUPDATE" \
                  --retry 3 --retry-connrefused --max-time 400 \
                  http://192.168.86.239:3842/v1/update
                '''
            }
        }
    }
}
