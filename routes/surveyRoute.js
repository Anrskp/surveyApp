var express = require('express');
var router = express.Router();
var p = require('../rabbitMQ/publisher');


/* rabbit MQ sender */
module.exports.sendSomething = (survey) => {

  var publisher = new p();

  try {
      publisher.publish(survey);
  } catch (err) {
      console.trace(err);
  }
  console.log('tried to send something to the que');

}

/*
router.post('/signup', function (req, res, next) {

    var publisher = new p();

    // send the message to teh publisher service
    //console.log(req.body);
    // return the user to the front page
    try {
        publisher.publish(req.body);
    } catch (err) {
        console.trace(err);
    }
    res.json('OK');
});
*/
//module.exports = router;
