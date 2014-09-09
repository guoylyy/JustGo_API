import os
import time
import hashlib
from flask.ext.restful import fields, marshal, abort

from datetime import datetime, timedelta


def test():
    aa = make_token(1)
    print check_expire(aa)

if __name__ == '__main__':
    test()