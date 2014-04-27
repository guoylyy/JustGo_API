from django.views.decorators.csrf import csrf_exempt
from django.http import HttpResponse
from user.models import *
from uploadfile.models import *
from config import get_config
import simplejson

import hashlib
import datetime
import time

# Create your views here.

@csrf_exempt
def register(request):
    # TODO: prevent DDoS register
    # TODO: password encrypted in the app
    try :
        username = request.POST.get('username')
        email = request.POST.get('email')
        password = request.POST.get('password')
        print request.read()
        default_portrait = __get_default_portrait()
        new_user = User(name = username, password = password, portrait = default_portrait, email = email)
        new_user.save()
        if new_user.id is not None:
            # perform login method once registered
            return __perform_login(new_user)
    except Exception as e:
        print (e)
        return HttpResponse(__get_result('exception occurs'))


@csrf_exempt
def login(request):
    try :
        username = request.POST.get('username')
        password = request.POST.get('password')

        users = User.objects.filter(name=username)
        if len(users) > 0:
            user = users[0]
            if user.password == password:
                return __perform_login(user)
            else:
                return HttpResponse(__get_result('wrong password'))
        else:
            return HttpResponse(__get_result('no user'))
    except Exception as e:
        print (e)
        return HttpResponse(__get_result('fail'))

@csrf_exempt
def login_status(request):
    try :
        token = request.POST.get('token')
        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))
            return HttpResponse(__get_result('success'))
        else:
            return HttpResponse(__get_result('fail'))
    except Exception as e:
        print (e)
        return HttpResponse(__get_result('fail'))

@csrf_exempt
def logout(request):
    try :
        token = request.POST.get('token')
        if token is not None or token != '':
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))
            rst = UserToken.objects.filter(token = token).delete()
            return HttpResponse(__get_result('success'))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e:
        print (e)
        return HttpResponse(__get_result('fail'))

def data_pull(request):
    try :
        token = request.GET.get('token')
        if token is not None or token != '':
            # check token
            if __check_token(token) is not True:
                return HttpResponse(__get_result('invalid token'))
            if __check_expire(token) is not True:
                return HttpResponse(__get_result('expired token'))
            md5, expire, user_id = token.split(":", 3)
            users = User.objects.filter(id=user_id)
            if len(users) == 0:
                return HttpResponse(__get_result('fail'))

            # render user data
            user, data = users[0], {}
            data['username'] = user.name
            data['email'] = user.email
            portrait = user.portrait
            data['portrait_path'] = portrait.file_name
            return HttpResponse(simplejson.dumps(str(data)))
        else :
            return HttpResponse(__get_result('fail'))
    except Exception as e:
        print (e)
        return HttpResponse(__get_result('fail'))

######################################################
## private functions start here

def __get_token_result(result,token):
    return str("{\"result\":\""+result+"\",\"token\":\""+token+"\"}")

def __get_result(result):
    return str("{\"result\":\""+result+"\"}")

def __get_error(msg_num):
    return str("{\"error\":\""+msg_num+"\"}")

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


def __insert_token(id, token):
    '''
    return True if store successfully
    '''
    try :
        new_token = UserToken(user_id = id, token = token)
        new_token.save()
        if new_token.id is not None:
            return True
        print ('Error: token insert fail!')
        return False
    except Exception as e:
        print (e)
        return False

def __check_token(token):
    '''
    return
    - True for matched token
    - False for invalid token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        tokens = UserToken.objects.filter(user_id = id)
        if (len(tokens) > 0):
            return True
        print ('Token auth Fail: id %s user token not match' % id)
        return False
    except Exception as e:
        print (e)
        return False

def __check_expire(token):
    '''
    return 
    - True for expire token
    - False for valid token
    - None for error token
    '''
    try :
        md5, expire, id = token.split(":", 3)
        if int(expire) < time.time():
            return False
        return True
    except Exception as e:
        print (e)
        return None

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
    token = __make_token(user.id)
    save_result = __insert_token(user, token)
    if save_result is True:
        return HttpResponse(__get_token_result('success',token))
    else:
        print ('Error: register save token fail!')
        return HttpResponse(__get_result('save token fail'))
