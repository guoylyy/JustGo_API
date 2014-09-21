# -*- coding: utf-8 -*-

import os
import sys
import unittest
import tempfile

from flask import current_app
from flask.ext.script import Manager,prompt,prompt_pass,\
        prompt_bool,prompt_choices, Server, Shell, Command, prompt_bool
from datetime import datetime, time, date
from app.models import Category, Goal, GoalJoin, GoalTrack

from sqlalchemy_imageattach.entity import Image, image_attachment, store_context
from app.extensions import db, fs_store
from app import create_app
from app import configs

from tests import B_GoalTest, A_UserTest, C_NotificationTest

manager = Manager(create_app(configs.TestConfig,'test_app',True))

@manager.command
def create_all():
    db.create_all()
    make_test_data()
    #goal_join_test_data()

@manager.command
def drop_all():
    db.drop_all()

@manager.command
def refresh():
    db.drop_all()
    db.create_all()
    make_test_data()
    #goal_join_test_data()


def make_test_data():
    categories = ['Popular', 'Health Diet', 'Train plans', 'Habits', 'Learning']
    for cat in categories:
        db.session.add(Category(cat, ''))
    goals = [
            ['Drink water','description','Popular'],
            ['Make love Every Day','description','Popular'],
            ['Write article ','description','Popular'],
            ['Do fitness','description','Health Diet'],
            ['Watch Movie','description','Train plans']
        ]
    db.session.commit()
    c = Category.query.filter(Category.category_name=='Popular').first()

    g_list = []
    with store_context(fs_store):
        for g in goals:
            goal = Goal(g[0], g[1])
            with open('pic1.jpg','rb') as f:
                goal.image.from_blob(f.read())
            g_list.append(goal)
            db.session.add(goal)
        c.goals = g_list
        db.session.add(c)
        db.session.commit()

def goal_join_test_data():
    gj = GoalJoin('fs332ab',1,1,7,'1,2,3,4,5',True,time(12,2), date(2014,8,8),date(2014,8,20), False)
    gt1 = GoalTrack('fs332ab',1, date(2014,8,9))
    gt2 = GoalTrack('fs332ab',1, date(2014,8,10))
    db.session.add(gj)
    db.session.add(gt1)
    db.session.add(gt2)
    db.session.commit()


if __name__ == '__main__':
    if len(sys.argv) ==1:
        unittest.main()
    else:
        manager.run()
