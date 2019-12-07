const net = require('net');
let client;
let characters = [];
let connected = false;

connectToGame = (host, port) => {
	// create socket
	client = new net.Socket();
	// connect to game
	client.connect(port, host, function() {
		console.log('Connected to game at ' + host + ':' + port);
		connected = true;
	});
	// update enemies array
	client.on('data', (data) => {
		handleData(data.toString());
	});
	// close connection
	client.on('close', function() {
		console.log('Connection closed');
		connected = false;
	});

    return true;
}

handleData = (data) => {
	let rawCharacters = JSON.parse(data.toString());
	let cleanCharacters = [];
	for (rawCharacter of rawCharacters) {
		cleanCharacters.push(mapEnemyObject(rawCharacter));
	}
	characters = cleanCharacters
}

mapEnemyObject = (data) => {
	let character = {};
	character.id = data.ID;
	character.type = mapCharacterType(data.Type);
	character.health = data.Health;
	character.weapon = mapInteractableType(data.WeaponType);
	character.ammo = data.Ammunition;
	character.position = mapPosition(data.Position);
	character.rotation = data.Rotation;
	character.interactables = mapInteractables(data);
	return character;
}

mapPosition = (pos) => {
    return {
        x: (pos.x).toFixed(2),
        y: (pos.y).toFixed(2)
    };
}

mapCharacterType = (identifier) => {
	switch (identifier) {
		case 'EASY': return 'Leicht';
		case 'HARD': return 'Schwer';
		case 'BASIC': return 'Standard';
		default: return 'Unbekannt';
	}
}

mapInteractables = (data) => {
	if (!data.VisibleElements || !data.VisibleElements.LevelItems) {
		return;
	}

	let rawInteractables = data.VisibleElements.LevelItems.Weapons;
	let interactables = [];

	for (item of rawInteractables) {
		if (item.CanInteract) {
			interactables.push({
				id: item.ID,
				type: mapInteractableType(item.Type)
			});
		}
	}

	return interactables;
}

mapInteractableType = (identifier) => {
	switch (identifier) {
		case 'BAT': return 'Schlagstock';
		case 'MACHINEGUN': return 'Maschinengewehr';
		case 'SHOTGUN': return 'Flinte';
		case 'PISTOL': return 'Pistole';
		default: return 'Keine';
	}
}

executeAction = (name, id) => {
    command = {
        'EnemyID': id,
        'Method': name
    }
    client.write(JSON.stringify(command));
    return true;
}

module.exports = {
    getAll (req, res) {
        res.send(characters);
    },
    connect (req, res) {
        console.log('Connecting...')
        if(connectToGame(req.params.ip, req.params.port)) {
            res.send(true);
        } else {
            res.status(400).end();
        }
    },
    action (req, res) {
        console.log('Executing ' + req.params.name + '...');
        if(executeAction(req.params.name, req.params.id)) {
            res.send(true);
        } else {
            res.status(400).end();
        }

    }
}
