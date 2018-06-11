 // require wascally
var mq = require('wascally');

// get the configuration
var config = require('./../config/rabbitMQconf');

function publisher (){

}

/**
* Creates the connection to RabbitMQ and assigns a message handler that
* will publish the messages
*/
publisher.prototype.publish = function(msg){
    mq.configure(config)
        .then(this.sendMessage(msg))
        .then(undefined, this.reportErrors);
};

publisher.prototype.reportErrors = function(err){
    console.log(err.stack);
    process.exit();
};

/**
* Send the actual message
*/
publisher.prototype.sendMessage = function sendMessages(msg) {

    mq.publish(config.exchanges[0].name, {
        type: "signup.incoming.type",
        routingKey: "",
        body: msg
    });
};

module.exports = publisher;
