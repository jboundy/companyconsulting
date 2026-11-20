To generate the client api. Run the web api project to generate the swagger documentation. Then run the command below to generate the local client

kiota generate --openapi http://localhost:5225/swagger/v1/swagger.json --language typescript --class-name productWebApi --namespace-name ApiClient --output ./src/api
