# -*- coding: utf-8 -*-

import unittest
from app import create_app
from app import configs

class BaseTest(unittest.TestCase):
    def setUp(self):
        print 'This is setup function'
        self.app = create_app(configs.TestConfig,'test_app',True)
        self.app.config['TESTING'] = True
        self.client = self.app.test_client()
        self.base_url = '/' + self.app.config['VERSION']

    def tearDown(self):
        print 'Close'

class GoalTest(BaseTest):

    def test_get_category(self):
        rep = self.client.get(self.base_url+'/goal_category')
        print rep.data

    def test_get_goal_from_category(self):
        rep = self.client.get(self.base_url+'/goal/Popular')
        print rep.data

    