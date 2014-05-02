"""
Django settings for JustGo_Backend project.

For more information on this file, see
https://docs.djangoproject.com/en/1.6/topics/settings/

For the full list of settings and their values, see
https://docs.djangoproject.com/en/1.6/ref/settings/
"""

# Build paths inside the project like this: os.path.join(BASE_DIR, ...)
import os
BASE_DIR = os.path.dirname(os.path.dirname(__file__))
MEDIA_ROOT = os.path.join(BASE_DIR,'media')


# Quick-start development settings - unsuitable for production
# See https://docs.djangoproject.com/en/1.6/howto/deployment/checklist/

# SECURITY WARNING: keep the secret key used in production secret!
SECRET_KEY = 'jh3f5h41flo08bv(8z)%b*t^x%lm&s(iawh)&r30h&4om3o0cz'

# SECURITY WARNING: don't run with debug turned on in production!
DEBUG = True

TEMPLATE_DEBUG = True

ALLOWED_HOSTS = []


# Application definition

INSTALLED_APPS = (
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    'users',
    'goal',
)

MIDDLEWARE_CLASSES = (
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
)

TEMPLATE_DIRS = {
    os.path.join(BASE_DIR,'templaete')
}

ROOT_URLCONF = 'JustGo_Backend.urls'

WSGI_APPLICATION = 'JustGo_Backend.wsgi.application'


# Database
# https://docs.djangoproject.com/en/1.6/ref/settings/#databases

DATABASES = {
    'default': {
         'ENGINE': 'django.db.backends.mysql',
         'NAME': 'justgo',
         'USER': 'wptest',
         'PASSWORD': 'wptest',
         'HOST': '115.28.4.78',
         'PORT': '3306',
    }
}

# Internationalization
# https://docs.djangoproject.com/en/1.6/topics/i18n/

LANGUAGE_CODE = 'en-us'

TIME_ZONE = 'UTC'

USE_I18N = True

USE_L10N = True

USE_TZ = True


# Static files (CSS, JavaScript, Images)
# https://docs.djangoproject.com/en/1.6/howto/static-files/
STATIC_URL = '/static/'


GGING = {
        'version': 1,
        'disable_existing_loggers': False,
    'filters': {
        'require_debug_false': {
                        '()': 'django.utils.log.RequireDebugFalse'
                    
        }
            
    },
    'handlers': {
        'null':{
                        'level': 'DEBUG',
                        'class': 'logging.NullHandler',
                    
        },
        'mail_admins': {
                        'level': 'ERROR',
                        'filters': ['require_debug_false'],
                        'class': 'django.utils.log.AdminEmailHandler'
                    
        }
            
    },
    'loggers': {
        'django': {
                        'handlers': ['null'],
                        'level': 'INFO',
                        'propagate': True,
                    
        },
        'django.request': {
                        'handlers': ['mail_admins'],
                        'level': 'ERROR',
                        'propagate': False,
                    
        },
            
    }

}

