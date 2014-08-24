The api requirement of WP.

---
###0. 系统参数

```
基础地质：
数据库信息：
```

###0.2 系统错误消息代码
系统消息代码是系统前后台用户通信错误信息的口令，可以方便统一消息意义

```
error_codes ={
	#global
	'200' : {"msg_en": 'Success',"msg_cn":""},
	'400' : {"msg_en": 'Bad request',"msg_cn":""},
	'401' : {"msg_en": 'You are not authoried to access this service',"msg_cn":""},
	'403' : {"msg_en": 'Forbidden by server',"msg_cn":""},
	'404' : {"msg_en": 'Service not found',"msg_cn":""},
	'414' : {"msg_en": 'Prameters missing',"msg_cn":""},
	'500' : {"msg_en": 'Internal error happen',"msg_cn":""},

	#login 
	'001' : {"msg_en": "Password is not correct","msg_cn":""}, 
	'002' : {"msg_en": "No this user","msg_cn":""}, 
	#register
	'011' : {"msg_en": "Email is exist","msg_cn":""},
	'012' : {"msg_en": "Password is too simple","msg_cn":""},
	'013' : {"msg_en": "","msg_cn":""},
	'014' : {"msg_en": "","msg_cn":""},
	#token
	'021' : {"msg_en":"Token is expired","msg_cn":""},
	'022' : {"msg_en":"Invalid token","msg_cn":""},
	'023' : {"msg_en":"Save token fail","msg_cn":""},

	#goal
	'031' : {"msg_en":"Wrong category name","msg_cn":""},
	'032' : {"msg_en":"Can't join this goal","msg_cn":""},
}

```


##1. Users API

###1.1 Fackbook注册API
(未完成)利用fackbook api注册并登陆
注册成功返回token，否则返回错误代码。
（注：这个方法应该整合在登录方法中，即如该fackbook账户已经注册过了，直接返回token，这个方法应该最后是隐藏在登录接口中，目前可以用微博方法来测试）

```
url: user/register
method: post
```
####数据说明
```
发送数据
	username:"xxxx"
	password:"xxxx"
	email:"xxx@xx.com"

返回数据：
	{"code":"200","result":{"token":"1d859c9ea10b754d1686e89423291399:1406980314:1"}}
```

###1.2 登陆API
(已完成)用户登陆，即重新刷新token，依然用已经fackbook账户登录。 需要调用facebook api做认证，感觉略微有点麻烦。
（如果没有注册，也应该调用这个方法）

```
url: user/login
method: post
```

####数据说明

```
发送数据
	username: "xxx"
	password: "xxxx"

返回数据
	{"code":"200","result":{"token":"1d859c9ea10b754d1686e89423291399:1406980314:1"}}

```


###1.3 测试用户token是否失效API
(已完成) RT

```
url: user/login_status
method: post
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	{"code":"200","result":{"isExperiod":false}}
```

###1.4 登出API
(已完成) RT,登出用户信息，用户的token被清空，下次登录需要再次从fackbook登录

```
url: user/logout
method: post
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	{"code":"200","result":NULL}
```

###1.5 获取用户数据API
(未完成) RT（头像、用户名、昵称以及个人简介）

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	{	"code":"200",
		"result":{
			"nickname":"xxx",
			"token":"xxxx",
			"avator": "fsdfasf",
			"brief_info" : ""
		}
	}
```

###1.6 查询用户API
输入用户名，进行模糊匹配查询


```
url: user/search
method: post
```
####数据说明
```
发送数据
	token:"xxxx"
	username: "金将军"

返回数据：
	{	"code":"200",
		"result":[
			{
				"nickname":"大金将军",
				"uid":"fasdsgdsgsag dsg dsa",
				"avator": "fdsfa"
			},
			{
				"nickname":"金将军2号",
				"uid":"fasdsgdsgsag dsg dsa",
				"avator": "fdsfa"
			}
		]
	}
```

###1.7 列出粉丝API
获取用户粉丝列表

```
url: user/list/followers
method: post
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	{	"code":"200",
		"result":[
			{
				"nickname":"大金将军",
				"uid":"fasdsgdsgsag dsg dsa",
				"avator": "fdsfa"
			},
			{
				"nickname":"金将军2号",
				"uid":"fasdsgdsgsag dsg dsa",
				"avator": "fdsfa"
			}
		]
	}
```


###1.8 列出关注用户API
获取用户所关注的人

```
url: user/list/followings
method: post
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	{	"code":"200",
		"result":[
			{
				"nickname":"大金将军",
				"uid":"fasdsgdsgsag dsg dsa",
				"avator": "fdsfa"
			},
			{
				"nickname":"金将军2号",
				"uid":"fasdsgdsgsag dsg dsa",
				"avator": "fdsfa"
			}
		]
	}
```



##2. Goals API

###2.1 加入目标API
(未完成) 用户加入某个目标，加入目标需要

```
url: goal/join
method: post
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```


###2.2 退出目标API
(未完成) 放弃和退出一个目标，退出过后未来将不再提醒，目前不支持以往的目标追溯功能，但是数据是会保存下来的。

```
url: goal/exit
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.3 同步目标API
(未完成) 拉取服务器上该用户的所有加入的goal数据。

```
url: goal/sync_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.4 上传最新用户数据API
(未完成) 上传最新用户goal数据，进行服务器同步，服务器根据目标更新的时间戳进行判断（这里可能会有问题）。

```
url: goal/sync_push
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.5 获取目标所有CategoryAPI
(未完成) RT

```
url: goal/category
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.6 获取所有目标API
(未完成) RT

```
url: goal/goal_list
method: get
```
####数据说明
```
发送数据
	token:"xxxx"
	groupid:""   //传入2.5获取的某一个id

返回数据：
	todo
```

###2.7 获取goal详细信息API
(未完成) RT

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```


###2.8 获取特定目标相关状态信息API
(未完成) RT 每次列出5个

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.9 获取目标的评论列表API
(未完成) RT

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.10 添加评论API
(未完成) RT 可以添加图片

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###2.11 点赞API
(未完成) RT

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```


###2.12 为特定目标发状态API
(未完成) RT

```
url: user/data_pull
method: get
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

##3. Notification API


###3.1 Get Notifications
(未完成) 拉取Notifications,需要包含已读或未读信息，在

```
url: notification
method:  get/put
```
####数据说明 post
发送notifications

```
发送数据
	token:"xxxx"

返回数据：
	{	"code":"200",
		"result":[
			{
				"title":"大金将军",
				"details":"fdsfasfsa",
				"avator": "fdsfa",
				"nickname" : "saf"
			},
			{
				"title":"大金将军",
				"details":"fdsfasfsa",
				"avator": "fdsfa",
				"nickname" : "saf"
			}
			....
		]
	}
```

###3.2 Mark as readed
(未完成) 拉取Notifications,标记已读

```
url: notification
method:  put
```
####数据说明 post
发送notifications

```
发送数据
	token:"xxxx"
	notification_id:"xxx,fdsf,fdsaf,fs"

返回数据：
	{"code":"200"}
```



###3.3 Encourage Notification
(未完成) 朋友发送鼓励消息

```
url: user/data_pull
method:  post
```

####数据说明 post
发送notifications

```
发送数据
	token:"xxxx"
	uid: "fdsaf"

返回数据：
	{"code":200}
```






