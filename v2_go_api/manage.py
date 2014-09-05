# -*- coding: utf-8 -*-
"""
	JustGo Project Backend Management Script
	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Manage the project

	:author: globit<yiliangg@foxmail.com>
	:copyrght: (c) 2014 by 404Design Workshop
	:license: BSD, see LICENSE for more details.
"""

import os
import sys
from flask import current_app
from flask.ext.script import Manager,prompt,prompt_pass,\
        prompt_bool,prompt_choices, Server, Shell, Command, prompt_bool

from app.extensions import db
from app import create_app

manager = Manager(create_app)

manager.add_command('runserver', Server())

def _make_context():
    return dict(db=db)

manager.add_command("shell", Shell(make_context=_make_context))

@manager.command
def create_all():
	db.create_all()

@manager.command
def drop_all():
	db.drop_all()

if __name__ == '__main__':
    manager.run()



