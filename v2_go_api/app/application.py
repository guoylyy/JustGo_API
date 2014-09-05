# -*- coding: utf-8 -*-


import os
import logging
from flask import Flask
from app import configs
from app.views import test_view as test

from flask.ext import restful

from extensions import db
from apis import create_api

from apis.user_api import * 
from apis.goal_api import * 

from sqlalchemy_imageattach.stores.fs import FileSystemStore


__all__ = ['create_app']

DEFAULT_APP_NAME = 'just_go'

REGISTER_BLUE_PRINTS = (
        (test.instance,''),
        # add your blue print here
        )

def create_app(config=None,app_name=None):
    
    if app_name is None:
        app_name = DEFAULT_APP_NAME
    
    app = Flask(app_name)

    configure_app(app, config)
    configure_extensions(app)
    confgiure_api(app)
    configure_blueprints(app)
    #configure_cache(app)
    return app

def configure_app(app,config):
    app.config.from_object(configs.DefaultConfig())
    if config is not None:
        app.config.from_object(config)
    app.config.from_envvar('APP_CONFIG', silent=True)

def configure_extensions(app):
    db.init_app(app)

def confgiure_api(app):
    BASE_URL = '/' + app.config['VERSION']
    api = create_api(restful.Api(app), BASE_URL)

def configure_blueprints(app):
    for blue,url_prefix in REGISTER_BLUE_PRINTS:
        #app.register_blueprint(blue)
        app.register_blueprint(blue,url_prefix=url_prefix)

    
