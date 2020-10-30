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


## LAB 2 - A sample Web Api that manipulates with avia tickets and warranties
### Used technologies
- .Net Core 3 + C#
- MongoDb as the main data storage
- Redis that is used for caching

### Get (filtered) list of tickets
#### Request
> Get all tickets: GET /api/tickets
> Get filtered tickets: GET /api/tickets?countryFrom=Russia&cityFrom=Moscow&countryTo=Ucraine&cityTo=Lviv&takeOffDay=1488-12-17T00:00:00.511Z
> **NOTE: Query parameters are handled dynamicaly. It's not required adding all of them at once.**
> Request Headers: Accept: application/json, application/xml

#### Response
> Accept: application/json
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

> Accept: application/xml
```xml
  <ArrayOfTicketModel xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/AviaSalesApiCopyTwo.Models.Tickets">
    <TicketModel>
        <ArriveOn>2020-12-19T20:30:00.511Z</ArriveOn>
        <CityFrom>Chisinau</CityFrom>
        <CityTo>Sydney</CityTo>
        <Company>Turkish Airlines, Malaysia Airlines</Company>
        <CountryFrom>Moldova</CountryFrom>
        <CountryTo>Australia</CountryTo>
        <Id>428c0d20-ee65-4546-8864-68aec6b5e50e</Id>
        <Price>900</Price>
        <TakeOffDate>2020-12-17T21:40:00.511Z</TakeOffDate>
        <TakeOffDay>2020-12-17T00:00:00.511Z</TakeOffDay>
        <TransitPlaces>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>Istanbul New Airport</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-17T16:50:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>Istanbul</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>Turkey</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-17T18:15:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>Kuala-Lumpur</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-17T22:30:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>Kuala-Lumpur</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>Malaysia</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-18T14:25:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
        </TransitPlaces>
    </TicketModel>
    <TicketModel>
        <ArriveOn>2020-12-19T07:10:00.511Z</ArriveOn>
        <CityFrom>Chisinau</CityFrom>
        <CityTo>Sydney</CityTo>
        <Company>Turkish Airlines, Wizz Air, Thai Airways</Company>
        <CountryFrom>Moldova</CountryFrom>
        <CountryTo>Australia</CountryTo>
        <Id>138b81d1-6392-4617-97c1-cf648480b2d1</Id>
        <Price>1000</Price>
        <TakeOffDate>2020-12-17T14:30:00.511Z</TakeOffDate>
        <TakeOffDay>2020-12-17T00:00:00.511Z</TakeOffDay>
        <TransitPlaces>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>Marco Polo</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-17T15:50:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>Venice</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>Italy</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-17T18:50:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>Istanbul New Airport</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-17T21:20:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>Istanbul</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>Turkey</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-17T23:50:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
            <TransitPlace>
                <_x003C_Airport_x003E_k__BackingField>Suvarnabhumi</_x003C_Airport_x003E_k__BackingField>
                <_x003C_ArriveDate_x003E_k__BackingField>2020-12-18T15:55:00.511Z</_x003C_ArriveDate_x003E_k__BackingField>
                <_x003C_City_x003E_k__BackingField>Bangkok</_x003C_City_x003E_k__BackingField>
                <_x003C_Country_x003E_k__BackingField>Thailand</_x003C_Country_x003E_k__BackingField>
                <_x003C_TakeOff_x003E_k__BackingField>2020-12-18T18:50:00.511Z</_x003C_TakeOff_x003E_k__BackingField>
            </TransitPlace>
        </TransitPlaces>
    </TicketModel>
</ArrayOfTicketModel>
```

#### Status Codes
- Status Code 200OK
- Status Code 404NotFound

### Get a ticket by Id
#### Request
> /api/tickets/ec25779a-c742-4a4e-be03-cf8aeec12490
> Request Headers: Accept: application/json, application/xml

#### Response
> Accept: application/json
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
> Accept: application/xml
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
- Status Code 200OK
- Status Code 404NotFound

