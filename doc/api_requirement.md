The api requirement of WP.

---
###0. 系统参数

```
基础地质：
数据库信息：

```



##1. Users API

###1.1 注册API
(已完成)注册并登陆

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
(已完成)用户登陆

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
	todo
```

###1.4 登出API
(已完成) RT

```
url: user/logout
method: post
```
####数据说明
```
发送数据
	token:"xxxx"

返回数据：
	todo
```

###1.4 获取用户数据API
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


##2. Goals API

###2.1 加入目标API
(未完成) 用户加入某个目标

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


###2.2 退出目标API
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

###2.3 同步目标API
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

###2.4 上传最新用户数据API
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

###2.5 获取目标所有分类API
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

###2.6 获取分类下所有目标API
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


#Template

###1.1 API
(已完成)xx

```
url: xx
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
