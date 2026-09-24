<script setup lang="ts">
import { Ticket } from '@lucide/vue'
import { Separator } from '@/components/ui/separator'
</script>

<template>
  <footer class="mt-auto bg-muted/30">
    <Separator />
    <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-8 flex flex-col sm:flex-row items-center justify-between gap-4">
      <div class="flex items-center gap-2 text-sm text-muted-foreground">
        <Ticket class="h-4 w-4 text-primary" />
        <span>&copy; {{ new Date().getFullYear() }} E-Ticket System. All rights reserved.</span>
      </div>
      <div class="flex gap-6 text-sm text-muted-foreground">
        <a href="#" class="hover:text-foreground transition-colors">Privacy Policy</a>
        <a href="#" class="hover:text-foreground transition-colors">Terms of Service</a>
        <a href="#" class="hover:text-foreground transition-colors">Support</a>
      </div>
    </div>
  </footer>
</template>
