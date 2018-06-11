module.exports = {
    "connection": {
        // credentials to connecto to rabbitMQ server.
        "user": "okigdyac",
        "pass": "qAAeul-Jo8naKIbhwMxFxtjwnCn8MLbP",
        "server": "sheep.rmq.cloudamqp.com",
        // if you have virtual hosts configured this is that option
        "vhost": "okigdyac"
    },
    // exchanges that we shall be talking to and their type.
    "exchanges": [
        {
            "name": "signup.main",
            "type": "direct"
        }
    ],

    "queues": [
        {
            "name": "signup-q.1"
        }
    ],

    // here we bind the exchanges and queues.
    "bindings": [
        {
            "exchange": "signup.main",
            "target": "signup-q.1"
        }
    ]
};
