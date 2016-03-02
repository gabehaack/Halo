var express         = require('express'),
    app             = express(),
    bodyParser      = require('body-parser'),
    mongoose        =   require('mongoose'),
	gamesController = require('./server/controllers/gamesController');


mongoose.connect('mongodb://localhost:27017/halo')

app.use(bodyParser());
app.use('/js', express.static(__dirname + '/client/js'));
app.use('/style', express.static(__dirname + '/client/style'));

app.get('/', function(req, res) {
    res.sendFile(__dirname + '/client/views/index.html');
});

app.get('/api/bestgames', gamesController.list)
app.post('/api/bestgames', gamesController.create);

app.listen(3000, function() {
    console.log('I\'m listening...');
})