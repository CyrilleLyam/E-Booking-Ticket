<script setup lang="ts">
import { Clock, LogIn, LogOut, Ticket, User } from '@lucide/vue'
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { revokeToken } from '@/lib/api/auth'
import { isAuthenticated as checkAuth, clearAuthTokens, getRefreshToken } from '@/lib/cookies'

const router = useRouter()
const isAuthenticated = ref(checkAuth())
const holdingTicketCount = ref(0)
const holdTimeRemaining = ref('08:45')

async function handleLogout() {
  const refreshToken = getRefreshToken()
  if (refreshToken) {
    try {
      await revokeToken(refreshToken)
    }
    catch {}
  }
  clearAuthTokens()
  isAuthenticated.value = false
  router.push('/login')
}
</script>

<template>
  <header class="sticky top-0 z-50 w-full border-b border-border/40 bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
    <div class="mx-auto flex h-16 max-w-7xl items-center justify-between px-4 sm:px-6 lg:px-8">
      <div class="flex items-center gap-6">
        <RouterLink to="/" class="flex items-center gap-2 font-bold text-xl tracking-tight">
          <div class="flex h-9 w-9 items-center justify-center rounded-lg bg-primary text-primary-foreground shadow-sm">
            <Ticket class="h-5 w-5" />
          </div>
          <span>E-Ticket</span>
        </RouterLink>

        <nav class="hidden md:flex items-center gap-6 text-sm font-medium">
          <RouterLink to="/" class="text-foreground/80 hover:text-foreground transition-colors">
            Events
          </RouterLink>
          <RouterLink to="/orders" class="text-muted-foreground hover:text-foreground transition-colors">
            My Bookings
          </RouterLink>
        </nav>
      </div>

      <div class="flex items-center gap-3">
        <Badge
          v-if="holdingTicketCount > 0"
          variant="secondary"
          class="hidden sm:flex items-center gap-1.5 px-3 py-1 text-xs border border-amber-500/20 bg-amber-500/10 text-amber-600 dark:text-amber-400"
        >
          <Clock class="h-3.5 w-3.5 animate-pulse" />
          <span>Seats held: {{ holdTimeRemaining }}</span>
        </Badge>

        <template v-if="isAuthenticated">
          <DropdownMenu>
            <DropdownMenuTrigger as-child>
              <Button variant="ghost" size="icon" class="rounded-full">
                <User class="h-5 w-5" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end" class="w-48">
              <DropdownMenuLabel>My Account</DropdownMenuLabel>
              <DropdownMenuSeparator />
              <DropdownMenuItem as-child>
                <RouterLink to="/profile" class="flex items-center gap-2 w-full cursor-pointer">
                  <User class="h-4 w-4" />
                  <span>Profile</span>
                </RouterLink>
              </DropdownMenuItem>
              <DropdownMenuItem as-child>
                <RouterLink to="/orders" class="flex items-center gap-2 w-full cursor-pointer">
                  <Ticket class="h-4 w-4" />
                  <span>My Tickets</span>
                </RouterLink>
              </DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem
                class="flex items-center gap-2 cursor-pointer text-destructive focus:text-destructive"
                @click="handleLogout"
              >
                <LogOut class="h-4 w-4" />
                <span>Log out</span>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </template>
        <template v-else>
          <Button variant="ghost" size="sm" as-child>
            <RouterLink to="/login">
              Sign in
            </RouterLink>
          </Button>
          <Button size="sm" class="gap-2" as-child>
            <RouterLink to="/register">
              <LogIn class="h-4 w-4" />
              <span>Get Started</span>
            </RouterLink>
          </Button>
        </template>
      </div>
    </div>
  </header>
</template>
