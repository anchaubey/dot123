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
- Linux(Ubuntu/Centos/Redhat) for deployment

## Installation & Code Setup

- Code checkout and build 

```
git clone https://github.com/realwear/Foresight-MS-DeviceManagement-RSystems.git -b Features/VL_NewProjectInitialCheckin

dotnet build
```
#### NOTE: -b refers to the branch name 

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

- Run all Integration Test collection to get the test summary.


## Build, Deploy & Integration Test on Kubernetes cluster with Helm


- Code Checkout

```
git clone https://github.com/realwear/Foresight-MS-DeviceManagement-RSystems.git -b Features/VL_NewProjectInitialCheckin
```

- Navigate to "Foresight-MS-DeviceManagement-RSystems" directory

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

## Configuration

- The default configuration has already been set into the configuraiton. Please follow the below steps to modify desired paramater according to the environment

- **KUBE_CONTEXT="to be updated here"**

- **NAMESPACE="to be updated here"**

- Next, we need to update the environment variables for dotnet service:-

```
--set env.ASPNETCORE__ENVIRONMEN="to be updated here"
--set env.Storage__MongoDBConnectionString="to be updated here"
--set env.Accounts__Authority="to be updated here"
--set env.Storage__MongoDBDatabaseName="to be updated here"
--set env.Storage__ServiceBusConnectionString="to be updated here"
```

- Execute integration.sh file 
```
./integration.sh
```

- This command will perform below operations

```
- Creates dotnet image with source code
- Deletes the namespace so that all the existing resources are deleted first, and then it would recreate the namespace.
- Mongodb service and pod will be created with the help of mongo_create.yaml and mongo_service.yaml files.
- Condition to ensure if the mongo service is running.
- Delete the existing dotnet deployment and recreate it with "helm upgrade".
- Once, all the dependency services are created, the final step is to build custom postman image and 
- run tests with the help of integration test collection file placed within "testharness" directory and produce below result.
```

- Integration Test Result

```
→ Setup_GetAntiForgeryToken
  GET https://foresightdev.realwear.com/account/login [200 OK, 7.43KB, 163ms]
  ✓  Status Code 200

→ Setup_GetToken_AllUsers
  POST https://foresightdev.realwear.com/account/token [302 Found, 1002B, 16ms]
  ✓  Status Code 302

→ Setup_GetToken_SuperAdmin
  POST https://foresightdev.realwear.com/account/token [302 Found, 1008B, 20ms]
  ✓  Status Code 302

→ CreateDevices_MinimumOneDeviceLimit_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 206B, 1084ms]
  ✓  Should contain at least 1 device

→ CreateDevices_SerialNumber_Required_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 395B, 32ms]
  ✓  Serial Number Required Validation

→ CreateDevices_SerialNumber_Regex_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 391B, 12ms]
  ✓  Serial Number Regex Validation

→ CreateDevices_Invalid _Email_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 384B, 6ms]
  ✓  Invalid Email Address

→ CreateDevices_Name_Length_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 400B, 7ms]
  ✓  Name cannot be more than 40 characters

→ CreateDevices_Description_Length_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 415B, 7ms]
  ✓  Description Max Length Validation

→ CreateDevices_MaxDevice_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 199B, 8ms]
  ✓  Request should not contain more than 100 devices

→ CreateDevices_Duplicate_Validation
  POST http://integration-devicemanagement:80/api/devices [400 Bad Request, 185B, 8ms]
  ✓  Serial Number Duplicate Validation

→ CreateDevices_Success
  GET http://integration-devicemanagement:80/api/devices?search=12345678910 [200 OK, 180B, 280ms]
  ┌
  │ { id: '4fe67e4a-5289-4a19-a8dc-cfd66c3dedc2',
  │   status: 'OK',
  │   code: 200,
  │   header:
  │    [ { key: 'Date',
  │        value: 'Mon, 01 Feb 2021 09:00:07 GMT' },
  │      { key: 'Content-Type',
  │        value: 'application/json; charset=utf-8' },
  │      { key: 'Server', value: 'Kestrel' },
  │      { key: 'Transfer-Encoding', value: 'chunked' } ],
  │   stream:
  │    { type: 'Buffer',
  │      data:
  │       [ 123,
  │         34,
  │         100,
  │         101,
  │         118,
  │         105,
  │         99,
  │         101,
  │         115,
  │         34,
  │         58,
  │         91,
  │         93,
  │         44,
  │         34,
  │         110,
  │         101,
  │         120,
  │         116,
  │         67,
  │         117,
  │         114,
  │         115,
  │         111,
  │         114,
  │         34,
  │         58,
  │         110,
  │         117,
  │         108,
  │         108,
  │         125 ] },
  │   cookie: [],
  │   responseTime: 280,
  │   responseSize: 32 }
  └
  POST http://integration-devicemanagement:80/api/devices [204 No Content, 81B, 1395ms]
  ✓  Device Created Successfully

→ CreateDevices_VerifyConflict_Validation
  POST http://integration-devicemanagement:80/api/devices [409 Conflict, 169B, 23ms]
  ✓  Serial Number cannot be duplicated

→ GetDevice_MinimumSearchLength_validation
  GET http://integration-devicemanagement:80/api/devices?search=1234 [400 Bad Request, 191B, 7ms]
  ✓  Get Devices Minimum Search Length validation

→ GetDevice_Created_Success
  GET http://integration-devicemanagement:80/api/devices?search=12345678910 [200 OK, 449B, 12ms]
  ✓  Get Id of Device
  ✓  Created Device Found successfully

→ GetAllDevices_SuperAdmin_MinimumSearchLength_Validation
  GET http://integration-devicemanagement:80/api/alldevices?search=1234 [400 Bad Request, 191B, 13ms]
  ✓  Get All Devices Minimum Search Length validation

→ GetAllDevices_Limit_From_Validation
  GET http://integration-devicemanagement:80/api/alldevices?from=XXX&limit=XXX&search= [400 Bad Request, 441B, 9ms]
  ✓  response is Bad Request
  ┌
  │ 'from=XXX&limit=XXX&search='
  │ 'from'
  │ 'XXX'
  │ 'limit'
  │ 'XXX'
  │ 'search'
  │ ''
  │ [ 'The value \'XXX\' is not valid for From.' ]
  └
  ✓  Invalid Limit and From Parameter

→ GetAllDevices_SuperAdmin_Success
  GET http://integration-devicemanagement:80/api/alldevices?from=0&limit=1&search=12345678910 [200 OK, 449B, 15ms]
  ┌
  │ 'from'
  │ '0'
  │ 'limit'
  │ '1'
  │ 'search'
  │ '12345678910'
  └
  ✓  limit should be null or an Integer Value and should not greater than 500
  ✓  response is ok
  ✓  response is valid and have a body
  ┌
  │ null
  │ 'Length1'
  └
  ✓  response has valid nextCursor

→ GetAllDevices_OtherUser_Failure
  GET http://integration-devicemanagement:80/api/alldevices?from=0&limit=5&search= [403 Forbidden, 99B, 10ms]
  ✓  Only SuperAdmin can access

→ UpdateDevice_Name_Length_Validation
  PUT http://integration-devicemanagement:80/api/devices/6017c318af1f21c7f82e749b [400 Bad Request, 389B, 16ms]
  ✓  Name cannot be more than 40 characters

→ UpdateDevice_Description_Length_Validation
  PUT http://integration-devicemanagement:80/api/devices/6017c318af1f21c7f82e749b [400 Bad Request, 404B, 7ms]
  ✓  Description Max Length Validation

→ UpdateDevice_Email_Validation
  PUT http://integration-devicemanagement:80/api/devices/6017c318af1f21c7f82e749b [400 Bad Request, 373B, 7ms]
  ✓  Invalid Email Address - Update

→ UpdateDevice_DeviceNotFound_Validation
  PUT http://integration-devicemanagement:80/api/devices/6012e6d2f57d2f724a7003d4 [404 Not Found, 165B, 23ms]
  ✓  Device not found - Update

→ UpdateDevice_Success
  PUT http://integration-devicemanagement:80/api/devices/6017c318af1f21c7f82e749b [204 No Content, 81B, 1051ms]
  ✓  Device Update Success

→ DeleteDevice_DeviceNotFound_Validation
  DELETE http://integration-devicemanagement:80/api/devices/6012c6d2e57d2f724a7003d4 [404 Not Found, 165B, 21ms]
  ✓  Device not found - Delete

→ DeleteDevice_Success
  DELETE http://integration-devicemanagement:80/api/devices/6017c318af1f21c7f82e749b [204 No Content, 81B, 989ms]
  ✓  Device Delete Success

→ GetDeletedDevice_NotFound_Success
  GET http://integration-devicemanagement:80/api/devices?search=12345678910 [200 OK, 180B, 10ms]
  ✓  Get Id of Device
  ✓  Response has no data for the serial number

┌─────────────────────────┬────────────────────┬────────────────────┐
│                         │           executed │             failed │
├─────────────────────────┼────────────────────┼────────────────────┤
│              iterations │                  1 │                  0 │
├─────────────────────────┼────────────────────┼────────────────────┤
│                requests │                 28 │                  0 │
├─────────────────────────┼────────────────────┼────────────────────┤
│            test-scripts │                 54 │                  0 │
├─────────────────────────┼────────────────────┼────────────────────┤
│      prerequest-scripts │                 39 │                  0 │
├─────────────────────────┼────────────────────┼────────────────────┤
│              assertions │                 33 │                  0 │
├─────────────────────────┴────────────────────┴────────────────────┤
│ total run duration: 6.2s                                          │
├───────────────────────────────────────────────────────────────────┤
│ total data received: 9.65KB (approx)                              │
├───────────────────────────────────────────────────────────────────┤
│ average response time: 187ms [min: 6ms, max: 1395ms, s.d.: 393ms] │
└───────────────────────────────────────────────────────────────────┘

```

- Few commands to verify the running services

```
kubectl get deployment -n dmt (To check the running dotnet deployment in dmt namespace)
kubectl get po -n dmt (to check all the pods running in dmt namespace which should list both our dotnet and mongo pods)
kubectl get svc -n dmt (to list all the services with dmt namespace)
```
