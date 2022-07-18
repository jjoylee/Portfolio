<br>

## 📌 나라마다 다른 주소 입력 양식 제공 & 주소 저장하기

ex) 한국 주소 : 도로명 주소 + 세부 주소, 미국 주소 : 주 + 도시 + 세부 주소  
기존 한국에 맞춰서 개발된 주소 체계 변경 및 이에 따라 관리자 페이지 사용자 주소 표시 페이지 수정.

## 📌 해결해야 할 문제 
1. 나라마다 다른 주소 양식을 보여줘야 한다.
2. 나라마다 다른 주소 체계를 가지고 있다. 주문할 떄 주소를 받아 db에 저장해야 한다.  
3. 입력된 주소를 합쳐서 주소 포맷에 맞게 표시해야 한다.


## 📌 해결 

1. AngularJs templateUrl을 사용해 나라에 따라 다른 주소 입력 폼 html을 load.

``` javascript
 templateUrl: ['$Nation', function ($Nation) {
    var nation = $Nation.Nation; // 국가(KR, US, JP ...)
    return "common address form url" + '/Address.tmpl.' + nation + + '.html';
 }]
```

2. 주소를 json string 으로 변환 후 db에 저장해 어떤 나라의 주소라도 하나의 컬럼에 저장할 수 있다.

```C#

// attribute 생성
[AttributeUsage(AttributeTargets.Property)]
public class JsonParsingAttribute : Attribute { }
  
 
// model property에 attribute 선언
[JsonParsing]
public JObject Address { get; set; } = new JObject();


// form에서 json string으로 넘어온 주소 값 parsing 해서 set
model.Address = JObject.Parse(FormData.GetStrParam("txtAddr", "{}"));


// db에 insert 할 때 json string으로 변환
if (Attribute.IsDefined(prop, typeof(JsonParsingAttribute)))
{
    return GetJsonValue(prop, value);
}

private static object GetJsonValue(PropertyInfo prop, object value)
{
    if (string.IsNullOrEmpty(value as string))
    {
        return Activator.CreateInstance(prop.PropertyType);
    }
    else
    {
        if (!value.GetType().Equals(typeof(string))) throw new NotSupportedException($"value type must be string : {value.GetType()}");
             return JsonConvert.DeserializeObject(value as string, prop.PropertyType);
    }
}    

```

3. 주소 포맷을 db에 저장. 화면에 주소를 표시할 떄 convertor를 사용해 db에 저장한 주소 데이터(json address)를 주소 포맷에 맞춰 변환해 주소 .

``` javascript
    module.service("$tqAddrService", ['$Config', function ($TCon) {
        function jsonAddrToStrAddr(jsonAddrObj, name) {
            var strAddrFormat = $Config.AddressFormat[name];
            var escapedJsonAddrObj = getEscapedJsonAddrObj(jsonAddrObj);
            for (var key in escapedJsonAddrObj) {
                strAddrFormat = strAddrFormat.replaceAll("@" + key, escapedJsonAddrObj[key]);
            }
            return strAddrFormat.replaceAll("**", "@");
        }

        function getEscapedJsonAddrObj(jsonAddrObj) {
            var escapedJsonAddrObj = initJsonAddrObj();
            for (var key in escapedJsonAddrObj) {
                if (jsonAddrObj.hasOwnProperty(key)) {
                    escapedJsonAddrObj[key] = jsonAddrObj[key].replaceAll("@", "**");
                }
            }
            return escapedJsonAddrObj;
        }

        function initJsonAddrObj() {
            var jsonAddrObj = {};
            var keyArray = $TCon.System.JsonAddress.Key;

            for (var i = 0 ; i < keyArray.length ; i++) {
                jsonAddrObj[keyArray[i]] = "";
            }
            return jsonAddrObj;
        }

        return {
            toStrAddr: function (jsonAddr, name) {
                return jsonAddrToStrAddr(jsonAddr, name);
            }
        };
    }]);
```


``` C#
public class JsonAddrToAddrService : IJsonAddrToAddrService
    {
        [Autowire]
        public IJsonAddrToDicService JsonAddrToDicService { get; set; }

        public AddrItem Do(JObject jsonAddr)
        {
            var strAddrFormat = TCon.Val<JsonAddrInfo>("System", "JsonAddress").StrAddrFormat;
            var addr1Format = strAddrFormat.StrAddr1;
            var addr2Format = strAddrFormat.StrAddr2;
            var strAddr1 = ToStrAddr(jsonAddr, addr1Format);
            var strAddr2 = ToStrAddr(jsonAddr, addr2Format);
            return new AddrItem { Addr1 = strAddr1, Addr2 = strAddr2 };
        }

        private string ToStrAddr(JObject jsonAddr, string format)
        {
            var escapedJsonAddrDic = GetEscapedJsonAddrDic(jsonAddr);
            var strAddr = format;
            foreach (var data in escapedJsonAddrDic)
            {
                strAddr = strAddr.Replace($"@{data.Key}", data.Value);
            }
            return strAddr.Replace("**", "@");
        }

        private Dictionary<string, string> GetEscapedJsonAddrDic(JObject jsonAddr)
        {
            var escapedJsonAddrDic = JsonAddrToDicService.Do(jsonAddr);
            foreach (var data in escapedJsonAddrDic.ToList())
            {
                escapedJsonAddrDic[data.Key] = data.Value.Replace("@", "**");
            }
            return escapedJsonAddrDic;
        }
    }
}
```
