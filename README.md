# Device Management Service - Setup & Deployment Steps

## Introduction

This helps to setup the development environment and deploy the Device Management Microservice into Kubernetes cluster with Helm.

## Prerequisites
(Recommended Version)
- Visual Studio 2019/ VS Code 2019
- Mongodb 3.4.24
- Postman Client 8.0.3
- MiniKube 1.17+
- Docker 19.03+
- kubectl version 1.20+
- Helm Version: v3.5.1
- git cli


## Installation & Code Setup

- Code checkout and build 

```
git clone https://github.com/realwear/Foresight-MS-DeviceManagement-RSystems.git -b Features/VL_NewProjectInitialCheckin

dotnet build
```

- Change the appsettings.config from application root location for Authority, Service Bus & MongoDb configuration.

```json

  "Storage": {
    "MongoDBConnectionString": "",
    "MongoDBDatabaseName": "",
    "ServiceBusConnectionString": ""
  },
  "Accounts": {
    "Authority": ""
  },
```

- Run the application

```
dotnet run
```

- Open the Postman and Import the Integration Test collection file from root/testharness folder.

- Run all integration test collection to get the test summary.


## Deployment of automated build and Integration testing to Kubernetes with Helm



- Clone the code repository on the machine which has prerequisites installed with below command

```
git clone https://github.com/realwear/Foresight-MS-DeviceManagement-RSystems.git -b Features/VL_NewProjectInitialCheckin
```

- Switch to "Foresight-MS-DeviceManagement-RSystems" directory

```
cd Foresight-MS-DeviceManagement-RSystems/
```

- Check the "Foresight-MS-DeviceManagement-RSystems" directory with below command

```
ls -lthr
```

- The directory has below files

```

DockerfileHarness
README.md
RealWear.DeviceManagement/
devicemanagement/
integrations.sh*
mongo_create.yaml
mongo_service.yaml
testharness/
```

- In order to execute our test cases, we need to run integration.sh file script which has all the steps to create the prerequisites and also run integration test collection. 

This file also contains environment variables required to be updated below before running the script as listed:-

- **KUBE_CONTEXT="to be updated here"**

- **NAMESPACE="to be updated here"**

A context is a group of access parameters. Each context contains a Kubernetes cluster, a user, and a namespace. The current context is the cluster that is currently the default for kubectl : all kubectl commands run against that cluster. To find the current context on a machine run below command on a linux machine:-

```

cat $HOME/.kube/config | grep "current-context"
```

- Next, we need to update the environment variables for dotnet service:-

```
--set env.ASPNETCORE__ENVIRONMEN="to be updated here"
--set env.Storage__MongoDBConnectionString="to be updated here"
--set env.Accounts__Authority="to be updated here"
--set env.Storage__MongoDBDatabaseName="to be updated here"
--set env.Storage__ServiceBusConnectionString="to be updated here"
```

- Once the above variables are updated within integration.sh file, please execute the integration.sh script with below command

```
./integration.sh
```

This will execute following steps:-

```

Creates dotnet image with source code.
Deletes the namespace so that all the existing resources are deleted first, and then it would recreate the namespace.
Mongodb service and pod will be created with the help of mongo_create.yaml and mongo_service.yaml files.
Condition to ensure if the mongo service is running.
Delete the existing dotnet deployment and recreate it with "helm upgrade".
Once, all the dependency services are created, the final step is to build custom postman image and run tests with the help of integration test collection file places within "testharness" directory. Please place your integration test collection file within testharness/ directory.
```


- For any changes related to mongo service, please refer to either mongo_create.yaml or mongo_service.yaml files.

```

Few commands to verify the running services:-
1) kubectl get deployment -n dmt (To check the running dotnet deployment in dmt namespace)
2) kubectl get po -n dmt (to check all the pods running in dmt namespace which should list both our dotnet and mongo pods)
3) kubectl get svc -n dmt (to list all the services with dmt namespace)
```

**Also, please note that while running helm upgrade, there are few environment variables being directly passed within the command. The variables will be used by dotnet service ti interact with other services.**

- To run the test, execute below command from within Foresight-MS-DeviceManagement-RSystems directory:-

```
./integrations.sh
```


|**Kubernetes Configuration**  |
|`KUBE_CONTEXT` | KUBE_CONTEXT | `minikube`|
|`NAMESPACE`| Networking Namespace for cluster | `dmt`|


### Configure the Postman Collection
Postman Collection Resides in **testharness** folder. `RealWearDeviceManagementService_IntegrationTest.postman_collection` 
Refrences of this file is in `DockerfileHarness` and `integrations.sh` please make sure both files referring to correct `postman_collection` file.
