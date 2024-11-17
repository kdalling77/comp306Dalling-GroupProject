pipeline {
    agent any

    stages {

        stage('Checkout') {
            steps {
                // Checkout the code from the specified Git repository and branch
                checkout scmGit(branches: [[name: '*/master']], extensions: [], userRemoteConfigs: [[url: 'https://github.com/kdalling77/comp306Dalling-GroupProject.git']])
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Restores the NuGet packages for the .NET Core project
                sh 'dotnet restore'
            }
        }

        stage('Build Project') {
            steps {
                // Builds the project in Release configuration
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Publish Project') {
            steps {
                // Publishes the application to a specified output directory
                sh 'dotnet publish --configuration Release --output ./publish'
            }
        }
    }

}
