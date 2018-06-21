/*
const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const fs = require('fs');
*/

// Imports
const ChartController = require('./ChartController');
const amqp = require('amqplib/callback_api');
const connString = "amqp://okigdyac:qAAeul-Jo8naKIbhwMxFxtjwnCn8MLbP@sheep.rmq.cloudamqp.com/okigdyac";

/*
// Declare express variable
const app = express();

// CORS Middleware
app.use(cors());

//Body Parser Middleware
app.use(bodyParser.json());

app.use(express.static('public'));

// Set port number
const port = process.env.PORT || 9000;

// Start server
app.listen(port, () => {
  console.log('Server startet on port ' + port);
});
*/

// Connect and listen to rabbitMQ queue
amqp.connect(connString, function(err, conn) {
  conn.createChannel(function(err, ch) {
    var q = 'rpc_gen_graph';

    ch.assertQueue(q, {durable: false});
    ch.prefetch(1);
    console.log(' [x] Awaiting RPC requests');

    // Receive message
    ch.consume(q, function reply(msg) {

      console.log(" [x] Received request : ", msg.content.toString());


      let survey = JSON.parse(msg.content);

      let labels = [];
      let graphData = [];

      for(let i = 0; i < survey.Answers.length; i++) {
        labels.push(survey.Answers[i].AnswerText);
        graphData.push(survey.Answers[i].Responses);
      }


      // Get image png buffer
      ChartController.generateChart(labels, graphData).then(imageBuffer => {
        // Use base64
        var encodedBuffer = imageBuffer.toString('base64');

        // Send response
        ch.sendToQueue(msg.properties.replyTo,
          new Buffer(encodedBuffer),
          {correlationId: msg.properties.correlationId});

          ch.ack(msg);
      });
    });
  });
});
