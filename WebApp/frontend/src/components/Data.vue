<template>
<div id="data">
    <p class="font-weight-bold">
        Aktive Gegner: {{ enemies.length }}
    </p>
    <div class="enemy-data">
        <Card :key="enemy.id" :enemy="enemy" v-for="enemy in enemies" />
    </div>
</div>
</template>

<script>
import Card from './Card.vue'
export default {
  name: 'Data',
  components: {
    Card
  },
  props: {
    enemies: Array
  }
}
</script>
