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
##### ClientAction is an enum with the following values

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
