FROM mcr.microsoft.com/dotnet/core/sdk:2.1

WORKDIR /signature-api-client-dotnet

COPY . .
RUN dotnet restore

RUN mv ./Buypass_Class_3_Test4_CA_3.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_CA_3.crt 
RUN mv ./Buypass_Class_3_Test4_Root_CA.pem /usr/local/share/ca-certificates/Buypass_Class_3_Test4_ROOT_CA.crt 

RUN update-ca-certificates

ENTRYPOINT ["dotnet", "test", "-c", "Release"]
