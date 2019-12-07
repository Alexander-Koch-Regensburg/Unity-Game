<template>
    <div id="app" class="container">
        <div id="top">
            <h1 class="mb-4">HSP Web Interface</h1>
            <p>
                Diese Web-Applikation ist eine Demonstration der Machine-Learning Schnittstelle in einem 2D-Shooter, der im Sommersemester 2019 im Rahmen eines Hauptseminarprojektes an der OTH Regensburg enstanden ist.
            </p>
            <p class="font-weight-bold">
                <span>Status: </span>
                <span class="text-success" v-if="connected">Verbunden mit {{ host + ':' + port}}</span>
                <span class="text-danger" v-if="!connected">Nicht verbunden</span>
            </p>
        </div>
        <Connect :host="host" :port="port" v-if="!connected" />
        <Data :enemies="enemies" v-if="connected" />
    </div>
</template>

<script>
import socketService from "./services/socket.service";
import Data from './components/Data.vue'
import Connect from './components/Connect.vue'

export default {
    name: 'app',
    components: {
        Data,
        Connect
    },
    data: function () {
        return {
            host: '127.0.0.1',
            port: 3000,
            connected: false,
            enemies: []
        }
    },
    methods: {
        connect(host, port) {
            this.host = host;
            this.port = port;
            socketService.connect(host, port)
                .then((connected) => {
                    this.connected = connected;
                })
                .catch(() => {
                    this.connected = false;
                    alert("Verbindung fehlgeschlagen.\nÜberprüfe, ob das Backend läuft.");
                })
        },
        getEnemies() {
            if(this.connected) {
                socketService.getAll()
                    .then((enemies) => {
                        if (enemies) {
                            this.enemies = enemies;
                        }
                    })
                    .catch(() => {
                        this.connected = false;
                        alert("Verbindung abgebrochen.\n\nÜberprüfe, ob das Socket in Unity läuft und die IP Adresse stimmt. Diese sollte der Log-Ausgabe in Unity entsprechen.")
                    })
            }
        }
    },
    mounted () {
        window.setInterval(this.getEnemies, 1000);
    }
}
</script>

<style>
  @import './assets/bootstrap.min.css';
  @import './assets/styles.css';
</style>
