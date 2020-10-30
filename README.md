# Distributed systems
## LAB1 (part 1) - Message Broker App using TCP sockets
### Custom Protocol details
### Request Body
> Each request is sent with the following format:
```yaml
{
  "ClientId": 6F9619FF-8B86-D011-B42D-00CF4FC964FF,
  "ClientAction": 1,
  "SensorTypes": ["sensor1"],
  "Data": 1488
}
```

> ClientId is send by the broker after a client has initiated connection
#### ClientAction is an enum with the following values

- Send
- Subscribe
- Unsubscribe
- TopicList

### Response Body:
> Each response body is sent with the following format:
```yaml
  {
    "Message": {"sensor": "Sensor type", "data": 1488},
    "StatusCode": Code
  }
```

### Statuc Codes:
> Each response has it's specific Status Code:
- 0 (ClientAccepted): connection  has been estabilished successfully
- 1 (Success): operation has been successfully done
- 2 (Fail): indicates that an error happened on the broker side

> Response message from the response body will be empty if a Status code is equals to Fail


## LAB 2 - A sample Web Api that manipulates with avia Tickets and Warrants
### Used technologies
- .Net Core 3 + C#
- MongoDb as the main data storage
- Redis that is used for caching

### Implemented
- An API as a Data Warehouse
- Distributed Mongo database with three replica sets
- Reverse Proxy middleware
- Load balancing on the Proxy's level
- Redis caching on the Proxy's level
- Swagger Open API Documentation

## Tickets
### Get (filtered) list of Tickets
#### Request
Get all tickets: 
> GET /api/tickets
Get filtered tickets:
> GET /api/tickets?countryFrom=string&cityFrom=string&countryTo=string&cityTo=string&takeOffDay=date
#### NOTE: Query parameters are handled dynamicaly. It's not required adding all of them at once.
##### Request Headers:
> Accept: application/json, application/xml
#### Response
> content-type: application/json
```yaml
  [
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "countryFrom": "string",
    "cityFrom": "string",
    "countryTo": "string",
    "cityTo": "string",
    "takeOffDay": "2020-10-30T19:11:42.302Z",
    "takeOffDate": "2020-10-30T19:11:42.302Z",
    "arriveOn": "2020-10-30T19:11:42.302Z",
    "transitPlaces": [
      {
        "country": "string",
        "city": "string",
        "airport": "string",
        "arriveDate": "2020-10-30T19:11:42.302Z",
        "takeOff": "2020-10-30T19:11:42.302Z"
      }
    ],
    "company": "string",
    "price": 0
  }
  ]
```

> content-type: application/xml
```xml
  <ArrayOfTicketModel xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/AviaSalesApiCopyTwo.Models.Tickets">
    <TicketModel>
        <ArriveOn>2020-12-19T20:30:00.511Z</ArriveOn>
        <CityFrom>string</CityFrom>
        <CityTo>string</CityTo>
        <Company>string</Company>
        <CountryFrom>string</CountryFrom>
        <CountryTo>string</CountryTo>
        <Id>428c0d20-ee65-4546-8864-68aec6b5e50e</Id>
        <Price>0</Price>
        <TakeOffDate>2020-12-17T21:40:00.511Z</TakeOffDate>
        <TakeOffDay>2020-12-17T00:00:00.511Z</TakeOffDay>
        <TransitPlaces>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>string</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-17T16:50:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>string</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>string</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-17T18:15:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>string</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-17T22:30:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>string</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>string</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-18T14:25:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
        </TransitPlaces>
    </TicketModel>
</ArrayOfTicketModel>
```

#### Status Codes
- Status Code 200OK
- Status Code 404NotFound

### Get Ticket by Id
#### Request
> /api/tickets/ec25779a-c742-4a4e-be03-cf8aeec12490
##### Request Headers
> Accept: application/json, application/xml

#### Response
> content-type: application/json
```yaml
  {
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "countryFrom": "string",
  "cityFrom": "string",
  "countryTo": "string",
  "cityTo": "string",
  "takeOffDay": "2020-10-30T19:27:15.554Z",
  "takeOffDate": "2020-10-30T19:27:15.554Z",
  "arriveOn": "2020-10-30T19:27:15.554Z",
  "transitPlaces": [
    {
      "country": "string",
      "city": "string",
      "airport": "string",
      "arriveDate": "2020-10-30T19:27:15.555Z",
      "takeOff": "2020-10-30T19:27:15.555Z"
    }
  ],
  "company": "string",
  "price": 0
}
```
> content-type: application/xml
```xml
  <?xml version="1.0" encoding="UTF-8"?>
<TicketModel>
	<id>3fa85f64-5717-4562-b3fc-2c963f66afa6</id>
	<countryFrom>string</countryFrom>
	<cityFrom>string</cityFrom>
	<countryTo>string</countryTo>
	<cityTo>string</cityTo>
	<takeOffDay>2020-10-30T19:34:32.233Z</takeOffDay>
	<takeOffDate>2020-10-30T19:34:32.233Z</takeOffDate>
	<arriveOn>2020-10-30T19:34:32.233Z</arriveOn>
	<transitPlaces>
		<country>string</country>
		<city>string</city>
		<airport>string</airport>
		<arriveDate>2020-10-30T19:34:32.233Z</arriveDate>
		<takeOff>2020-10-30T19:34:32.233Z</takeOff>
	</transitPlaces>
	<company>string</company>
	<price>0</price>
</TicketModel>
```

#### Status Codes
- 200OK
- 404NotFound

### Create a new Ticket
#### Request
> POST /api/tickets
##### Headers
- content-type: application/json, application/xml
- accept: application/json, application/xml
#### Request body
> content-type: application/json
```yaml
{
  "countryFrom": "string",
  "cityFrom": "string",
  "countryTo": "string",
  "cityTo": "string",
  "takeOffDay": "2020-10-30T19:40:02.872Z",
  "takeOffDate": "2020-10-30T19:40:02.873Z",
  "arriveOn": "2020-10-30T19:40:02.873Z",
  "transitPlaces": [
    {
      "country": "string",
      "city": "string",
      "airport": "string",
      "arriveDate": "2020-10-30T19:40:02.873Z",
      "takeOff": "2020-10-30T19:40:02.873Z"
    }
  ],
  "company": "string",
  "price": 0
}
```
>content-type: application/xml
```xml
	<?xml version="1.0" encoding="UTF-8"?>
<TicketCreateUpdateModel>
	<countryFrom>string</countryFrom>
	<cityFrom>string</cityFrom>
	<countryTo>string</countryTo>
	<cityTo>string</cityTo>
	<takeOffDay>2020-10-30T19:41:32.770Z</takeOffDay>
	<takeOffDate>2020-10-30T19:41:32.770Z</takeOffDate>
	<arriveOn>2020-10-30T19:41:32.770Z</arriveOn>
	<transitPlaces>
		<country>string</country>
		<city>string</city>
		<airport>string</airport>
		<arriveDate>2020-10-30T19:41:32.771Z</arriveDate>
		<takeOff>2020-10-30T19:41:32.771Z</takeOff>
	</transitPlaces>
	<company>string</company>
	<price>0</price>
</TicketCreateUpdateModel>
```

#### Reponse
Status Codes
> 201Created
#### Response body
> content-type: application/json
```yaml
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "countryFrom": "string",
  "cityFrom": "string",
  "countryTo": "string",
  "cityTo": "string",
  "takeOffDay": "2020-10-30T19:41:32.801Z",
  "takeOffDate": "2020-10-30T19:41:32.801Z",
  "arriveOn": "2020-10-30T19:41:32.801Z",
  "transitPlaces": [
    {
      "country": "string",
      "city": "string",
      "airport": "string",
      "arriveDate": "2020-10-30T19:41:32.801Z",
      "takeOff": "2020-10-30T19:41:32.801Z"
    }
  ],
  "company": "string",
  "price": 0
}
```

> content-type: application/xml
```xml
	<?xml version="1.0" encoding="UTF-8"?>
<TicketModel>
	<id>3fa85f64-5717-4562-b3fc-2c963f66afa6</id>
	<countryFrom>string</countryFrom>
	<cityFrom>string</cityFrom>
	<countryTo>string</countryTo>
	<cityTo>string</cityTo>
	<takeOffDay>2020-10-30T19:44:17.945Z</takeOffDay>
	<takeOffDate>2020-10-30T19:44:17.945Z</takeOffDate>
	<arriveOn>2020-10-30T19:44:17.945Z</arriveOn>
	<transitPlaces>
		<country>string</country>
		<city>string</city>
		<airport>string</airport>
		<arriveDate>2020-10-30T19:44:17.946Z</arriveDate>
		<takeOff>2020-10-30T19:44:17.946Z</takeOff>
	</transitPlaces>
	<company>string</company>
	<price>0</price>
</TicketModel>
```

### Change a Ticket's state
#### Request
> PUT /api/tickets/ec25779a-c742-4a4e-be03-cf8aeec12490
##### Request Headers
> content-type: application/json, application/xml
#### Request body
```yaml
{
  "countryFrom": "string",
  "cityFrom": "string",
  "countryTo": "string",
  "cityTo": "string",
  "takeOffDay": "2020-10-30T19:54:33.531Z",
  "takeOffDate": "2020-10-30T19:54:33.532Z",
  "arriveOn": "2020-10-30T19:54:33.532Z",
  "transitPlaces": [
    {
      "country": "string",
      "city": "string",
      "airport": "string",
      "arriveDate": "2020-10-30T19:54:33.532Z",
      "takeOff": "2020-10-30T19:54:33.532Z"
    }
  ],
  "company": "string",
  "price": 0
}
```

#### Response
Status Codes
- 204NoContent
- 404NotFound

### Delete a Ticket
#### Request
> DELETE /api/tickets/ec25779a-c742-4a4e-be03-cf8aeec12490

#### Response
Status Codes
- 204NoContent
- 404NotFound

## Warrants
### Get Warrants by a passenger's iban
#### Request
> GET /api/warrants?iban=string
##### Request Headers
- accept: application/json, application/xml

#### Response
> content-type: application/json
```yaml
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "passengerIban": "string",
    "passportId": "string",
    "ticketId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ticketBackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "isPaid": true
  }
]
```
> content-type: application/xml
```xml
<ArrayOfWarrantModel xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/AviaSalesApiCopyTwo.Models.Warrants">
	<WarrantModel>
		<id>3fa85f64-5717-4562-b3fc-2c963f66afa6</id>
		<passengerIban>string</passengerIban>
		<passportId>string</passportId>
		<ticketId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketId>
		<ticketBackId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketBackId>
		<isPaid>true</isPaid>
	</WarrantModel>
</ArrayOfWarrantModel>
```
##### Status Codes
> 200OK

### Get a Warrant by Id
#### Request
> GET /api/warrants/ec25779a-c742-4a4e-be03-cf8aeec12490
##### Request Headers
- accept: application/json, application/xml

#### Response
> content-type: application/json
```yaml
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "passengerIban": "string",
    "passportId": "string",
    "ticketId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ticketBackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "isPaid": true
  }
```
> content-type: application/xml
```xml
	<?xml version="1.0" encoding="UTF-8"?>
	<WarrantModel>
		<id>3fa85f64-5717-4562-b3fc-2c963f66afa6</id>
		<passengerIban>string</passengerIban>
		<passportId>string</passportId>
		<ticketId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketId>
		<ticketBackId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketBackId>
		<isPaid>true</isPaid>
	</WarrantModel>
```
##### Status Codes
- 200OK
- 404NotFound

### Create a new Warrant
#### Request
> POST /api/warrants
##### Request Headers
- accept: application/json, application/xml
- content-type: application/json, application/xml
##### Request body
> content-type: application/json
```yaml
{
    "passengerIban": "string",
    "passportId": "string",
    "ticketId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ticketBackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "isPaid": true
 }	
```
> content-type: application/xml
```xml
	<?xml version="1.0" encoding="UTF-8"?>
	<WarrantModel>
		<passengerIban>string</passengerIban>
		<passportId>string</passportId>
		<ticketId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketId>
		<ticketBackId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketBackId>
		<isPaid>true</isPaid>
	</WarrantModel>
```

#### Response
> content-type: application/json
```yaml
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "passengerIban": "string",
    "passportId": "string",
    "ticketId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ticketBackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "isPaid": true
  }
```
> content-type: application/xml
```xml
	<?xml version="1.0" encoding="UTF-8"?>
	<WarrantModel>
		<id>3fa85f64-5717-4562-b3fc-2c963f66afa6</id>
		<passengerIban>string</passengerIban>
		<passportId>string</passportId>
		<ticketId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketId>
		<ticketBackId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketBackId>
		<isPaid>true</isPaid>
	</WarrantModel>
```
##### Status Codes
> 201Created

### Change a Warrant's state
#### Request
> PUT /api/warrants/ec25779a-c742-4a4e-be03-cf8aeec12490
##### Request Headers
> content-type: application/json, application/xml
##### Request Body
> content-type: application/json
```yaml
{
    "passengerIban": "string",
    "passportId": "string",
    "ticketId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ticketBackId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "isPaid": true
 }	
```
> content-type: application/xml
```xml
	<?xml version="1.0" encoding="UTF-8"?>
	<WarrantModel>
		<passengerIban>string</passengerIban>
		<passportId>string</passportId>
		<ticketId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketId>
		<ticketBackId>3fa85f64-5717-4562-b3fc-2c963f66afa6</ticketBackId>
		<isPaid>true</isPaid>
	</WarrantModel>
```

#### Response
##### Status Codes
- 204NoContent
- 404NotFound

### Delete Warrant
#### Request
> DELETE /api/warrants/3fa85f64-5717-4562-b3fc-2c963f66afa6

#### Response
##### Status Codes
- 204NoContent
- 404NotFound

## Get, Update or Delete a non-existing resource
#### Request
- GET /api/{resource}/ec25779a-c742-4a4e-be03-cf8aeec12499
- PUT /api/{resource}/ec25779a-c742-4a4e-be03-cf8aeec12499
- DELETE GET /api/{resource}/ec25779a-c742-4a4e-be03-cf8aeec12499
#### Response

```yaml
{
    "StatusCode": 404,
    "Error": "Entity of type {resource} with id: ec25779a-c742-4a4e-be03-cf8aeec12499 not found"
}
```
