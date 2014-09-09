import os
import app 
import unittest
import tempfile

class FlaskrTestCase(unittest.TestCase):
    def setUp(self):
        print "This is test"
        self.db_fd, app.app.config['DATABASE'] = tempfile.mkstemp()
        app.app.config['TESTING'] = True
        self.app = app.app.test_client()
    
    def tearDown(self):
        os.close(self.db_fd)
        os.unlink(app.app.config['DATABASE'])
    
    def get_token(self):
        return ""

    #--------------------Test functions --------------------------#
    def test_get_category(self):
        rv = self.app.get('/V1/goal_category')
        assert len(rv.data) > 0

    def test_get_goals(self):
        rv = self.app.get('/V1/goal_category/Popular')
        assert len(rv.data) > 0


if __name__ == '__main__':
    unittest.main()
