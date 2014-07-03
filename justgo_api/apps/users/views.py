from django.views.decorators.csrf import csrf_exempt
from django.http import HttpResponse
from apps.users.models import *
from apps.conf.config import get_config,get_page_result
from apps.conf.helper import check_token
import simplejson
import hashlib
import datetime
import time

@csrf_exempt
def register(request):
    try :
        username = request.POST.get('username')
        email = request.POST.get('email')
        password = request.POST.get('password')
        if len(User.objects.filter(email=email)) > 0:
            return HttpResponse(get_page_result('011'))
        #default_portrait = __get_default_portrait()
        new_user = User(name = username, password = password,  email = email)
        new_user.save()
        if new_user.id is not None:
            return __perform_login(new_user)
        else:
            return HttpResponse(get_page_result('500'))
    except Exception as e:
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def login(request):
    try :
        email = request.POST.get('email')
        password = request.POST.get('password')
        users = User.objects.filter(email=email)
        if len(users) > 0:
            user = users[0]
            if user.password == password:
                return __perform_login(user)
            else:
                return HttpResponse(get_page_result('001'))
        else:
            return HttpResponse(get_page_result('002'))
    except Exception as e:
        print(e)
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def login_status(request):
    try :
        token = request.POST.get('token')
        response = check_token(token)
        if response is True:
            return HttpResponse(get_page_result('200',"null"))
        else:
            return response
    except Exception as e:
        return HttpResponse(get_page_result('500'))

@csrf_exempt
def logout(request):
    try :
        token = request.POST.get('token')
        response = check_token(token)
        if response is True:
            user = User.objects.filter(auth_token=token)[0]
            user.auth_token = ''
            user.save()
            return HttpResponse(get_page_result('200',"null"))
        else:
            return response
    except Exception as e:
        print(e)
        return HttpResponse(get_page_result('500'))

def data_pull(request):
    try :
        token = request.GET.get('token')
        response = check_token(token)
        if response is True:
            md5, expire, user_id = token.split(":", 3)
            users = User.objects.filter(id=user_id)
            if len(users) == 0:
                return HttpResponse(get_page_result('404'))
            # render user data
            user, data = users[0], {}
            data['id'] = user.id
            data['username'] = user.name
            data['email'] = user.email
            portrait = user.portrait
            data['portrait_path'] = portrait.file_name
            return HttpResponse(get_page_result('200',simplejson.dumps(str(data))))
        else:
            return response
    except Exception as e:
        return HttpResponse(get_page_result('500'))

######################################################
# private functions start here
#
def __get_token_result(result,token):
    return str("{\"token\":\""+token+"\"}")


def __make_token(id):
    # generate expire date
    expire_day = get_config('session_expire')
    expire_time = (datetime.datetime.now() + datetime.timedelta(expire_day,0)).timetuple();
    expire_datetime = time.mktime(expire_time)
    expire = str(int(expire_datetime))
    # token
    s = '%s:%s:%s' % (id, expire, get_config('md5_random'))
    md5 = hashlib.md5(s.encode('utf-8')).hexdigest()

    return md5+":"+expire+":"+str(id)


def __get_default_portrait():
    '''
    return UploadFile object
    '''
    try :
        name = get_config('default_portrait_name')
        portrait = UploadFile(file_name=name)
        portrait.save()
        if portrait.id is not None:
            return portrait
        print ('Error: Default portrait save fail!')
        return None
    except Exception as e:
        print (e)
        return None

def __perform_login(user):
    '''
    generate token and return
    '''
    try:
        user.auth_token = __make_token(user.id) 
        user.save()
        return HttpResponse(get_page_result('200',__get_token_result('success',user.auth_token)))
    except Exception as e:
        print(e)
        return HttpResponse(get_page_result('023'))
# Private function ends
###############################################
