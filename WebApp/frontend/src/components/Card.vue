<template>
<div class="enemy-card">
    <div class="enemy-card--top d-flex p-4">
        <div class="enemy-data--type">
            <span class="font-weight-bold">Typ: </span>
            <span>{{ enemy.type }}</span>
        </div>
        <div class="enemy-data--health ml-4">
            <span class="font-weight-bold">Leben: </span>
            <span>{{ enemy.health }}</span>
        </div>
        <div class="enemy-data--position ml-4">
            <span class="font-weight-bold">Position: </span>
            <span>{{ enemy.position.x }}, {{ enemy.position.y }}</span>
        </div>
        <div class="enemy-data--weapon ml-4">
            <span class="font-weight-bold">Waffe: </span>
            {{ enemy.weapon ? enemy.weapon : 'NONE' }}
        </div>
        <div class="enemy-data--ammo ml-4">
            <span class="font-weight-bold">Munition: </span>
            {{ (enemy.ammo >= 0) ? enemy.ammo : '&infin;' }}
        </div>
    </div>
    <div class="enemy-card--middle p-4">
        <p class="font-weight-bold">
            Mögliche Interaktionen:
        </p>
        <p v-if="!enemy.interactables.length" class="mb-0">
            Keine Gegenstände in Reichweite
        </p>
        <div v-if="!enemy.interactables.length">
            <div class="interactable d-flex justify-content-between" v-bind:key="interactable" v-for="interactable in enemy.interactables">
                <div>
                    <span class="font-weight-bold">Gebrauchsgegenstand:</span>
                    <span>{{ interactable.type }}</span>
                </div>
                <button @click="interact(interactable.id)" class="btn-primary">Aufheben</button>
            </div>
        </div>
    </div>
    <div class="enemy-card--bottom p-4">  <!-- remove d-none when button-functionality is implemented -->
        <span class="font-weight-bold">Andere Aktionen:</span>
        <button @click="execute('DropWeapon')" class="btn-primary ml-4">Waffe fallen lassen</button>
        <button @click="execute('FireWeapon')" class="btn-primary ml-4">Waffe feuern</button>
        <button @click="execute('CloseCombatAttack')" class="btn-primary ml-4">Nahkampfangriff</button>
        <button @click="execute('StopOverride')" class="btn-primary ml-4">Kontrolle zurückgeben</button>
    </div>
</div>
</template>

<script>
import socketService from "../services/socket.service";
export default {
    name: 'Card',
    props: {
        enemy: Object
    },
    methods: {
        execute(name) {
            socketService.action(name, this.enemy.id);
        }
    }
}
</script>
