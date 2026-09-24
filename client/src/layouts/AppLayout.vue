<script setup lang="ts">
import Footer from './Footer.vue'
import Navbar from './Navbar.vue'
</script>

<template>
  <div class="min-h-screen flex flex-col bg-background text-foreground antialiased">
    <Navbar />

    <main class="flex-1 mx-auto w-full max-w-7xl px-4 sm:px-6 lg:px-8 py-8">
      <slot />
    </main>

    <Footer />
  </div>
</template>
