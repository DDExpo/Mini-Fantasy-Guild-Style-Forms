<script setup lang="ts">
  import { useRouter } from 'vue-router'
  const router = useRouter()
  const goHome = () => { router.push('/')}
</script>

<template>
  <link href="https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap" rel="stylesheet">
  <div class="not-found-container">
    <div class="not-found-content">
      <h2 class="error-code">404</h2>
      <p class="error-message">Page Not Found</p>
      <button type="button" @click="goHome" class="home-link">Go to Home</button>
    </div>
  </div>
</template>

<style scoped>

.not-found-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  width: 100%;
  background: url("/pixelBackground.webp") no-repeat center center fixed, #222;
  background-size: cover;
  image-rendering: pixelated;
  background-color: #1a1a1a; 
  color: #fff;
  text-align: center;
}

.not-found-content {
  background-color: rgba(0, 0, 0, 0.7);
  padding: 40px 60px;
  border: 4px solid #fff;
  box-shadow: 
    8px 8px 0px 0px rgba(0, 0, 0, 0.5),
    inset 0 0 0 2px rgba(255, 255, 255, 0.1);
  image-rendering: pixelated;
  transform: rotate(-2deg); 
}

.error-code {
  font-size: 13vw;
  line-height: 1;
  margin: 0;
  color: rgb(254, 231, 168);
  text-shadow: 
    -4px -4px 0px #000,
    4px 4px 0px #000,
    -4px 4px 0px #000,
    4px -4px 0px #000,
    0px 8px 0px #666;
  font-family: 'Press Start 2P', cursive;
  image-rendering: pixelated;
}

.error-message {
  font-size: 2em;
  margin-top: 20px;
  color: #eee;
  font-family: 'Press Start 2P', cursive;
  image-rendering: pixelated;
}

.home-link {
  display: inline-block;
  margin-top: 40px;
  padding: 15px 30px;
  background-color: rgb(254, 231, 168);
  color: #222;
  text-decoration: none;
  font-weight: bold;
  border: 2px solid #000;
  box-shadow: 4px 4px 0px #000;
  image-rendering: pixelated;
  transition: all 0.1s ease-out;
  font-family: 'Press Start 2P', cursive;
  font-size: 1em;
}

.home-link:hover {
  background-color: #fff;
  color: #000;
  box-shadow: 6px 6px 0px #000;
  transform: translate(-2px, -2px);
}

</style>
