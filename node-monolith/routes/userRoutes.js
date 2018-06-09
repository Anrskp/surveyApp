const express = require('express');
const router = express.Router();
const passport = require('passport');
const jwt = require('jsonwebtoken');
const User = require('../models/user');
const config = require('../config/config');

// Register
router.post('/register', (req, res, next) => {
  let newUser = new User({
    username: req.body.username,
    email: req.body.email,
    password: req.body.password
  });

  User.getUserByUsername(req.body.username, (err, user) => {
    if (err) throw err

    // check if username is avaiable
    if (user != null) {
      res.json({
        success: false,
        msg: 'username already in use'
      })
    } else {
      // check if email is avaiable
      User.getUserByEmail(req.body.email, (err, email) => {
        if (err) throw err
        if (email != null) {
          res.json({
            success: false,
            msg: 'email already in use'
          })
        } else {
          // Add the new user
          User.addUser(newUser, (err, user) => {
            if (err) {
              res.json({
                success: false,
                msg: 'Failed to register user'
              });
            } else {
              res.json({
                success: true,
                msg: 'User registered'
              });
            }
          });
        }
      })
    }
  })
});

// Authenticate
router.post('/authenticate', (req, res, next) => {
  const username = req.body.username;
  const password = req.body.password;

  // Check if user exsist
  User.getUserByUsername(username, (err, user) => {
    if (err) throw err;
    if (!user) {
      return res.json({
        success: false,
        msg: 'User not found'
      });
    }

    // Validate user password
    User.comparePassword(password, user.password, (err, isMatch) => {
      if (err) throw err;
      if (isMatch) {
        const token = jwt.sign({
          data: user
        }, config.secret, {
          expiresIn: 7200 // 2 hours
        });

        res.json({
          success: true,
          token: 'JWT ' + token,
          user: {
            id: user._id,
            email: user.email,
            username: user.username
          }
        });
      } else {
        return res.json({
          success: false,
          msg: 'Wrong password'
        });
      }
    });
  });
});

// Get user details
router.get('/profile', passport.authenticate('jwt', {
  session: false
}), (req, res, next) => {
  res.json({
    user: req.user
  })
});

module.exports = router;
