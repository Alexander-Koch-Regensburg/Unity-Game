<template>
<div id="connect">
    <p>
        Um die Applikation zu verwenden, stellen Sie bitte sicher, dass das Unity Spiel mit Socket läuft. Passen Sie gegebenenfalls IP Adresse und Port an und klicken Sie anschließend auf "Verbinden". Bei Problemen kann es hilfreich sein, die Internet-Verbindung zu deaktivieren.
    </p>
    <div class="connection">
        <input v-model="host" placeholder="IP Adresse">
        <input v-model="port" placeholder="Port">
        <button @click="connect()" class="btn-primary">Verbinden</button>
    </div>
</div>
</template>

<script>
export default {
    name: 'Connect',
    props: ['host', 'port'],
    methods: {
        connect() {
            this.$parent.connect(this.host, this.port);
        }
    }
}
</script>
