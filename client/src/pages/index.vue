<script setup lang="ts">
import type { EventQuery } from '@/types'
import { AlertCircle, Calendar, ChevronLeft, ChevronRight, LogIn, Sparkles, Users } from '@lucide/vue'
import { reactive } from 'vue'
import { RouterLink } from 'vue-router'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'
import { useEventsQuery } from '@/lib/api/event-queries'
import { isAuthenticated } from '@/lib/cookies'

const query = reactive<EventQuery>({
  page: 1,
  pageSize: 6,
})

const { data, isPending, error, refetch } = useEventsQuery(query)

function formatDate(isoString: string): string {
  if (!isoString) {
    return ''
  }
  return new Intl.DateTimeFormat('en-US', {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(isoString))
}

function handlePageChange(newPage: number) {
  query.page = newPage
}
</script>

<template>
  <AppLayout>
    <div class="space-y-10">
      <Card class="relative overflow-hidden bg-gradient-to-br from-primary/10 via-primary/5 to-background border-border p-6 md:p-10">
        <div class="max-w-2xl space-y-4">
          <Badge variant="outline" class="gap-1.5 px-3 py-1 text-primary border-primary/20 bg-primary/10">
            <Sparkles class="h-3.5 w-3.5" />
            <span>High-Demand Ticket Queue Enabled</span>
          </Badge>
          <h1 class="text-3xl font-extrabold tracking-tight sm:text-5xl">
            Book Live Events with Zero Friction
          </h1>
          <p class="text-base sm:text-lg text-muted-foreground">
            Fair virtual queues, instant seat reservations, and seamless ticket checkout.
          </p>
          <div class="flex flex-wrap gap-3 pt-2">
            <Button size="lg" as-child>
              <a href="#events">Explore Events</a>
            </Button>
            <Button variant="outline" size="lg" as-child>
              <RouterLink to="/login">
                Check My Bookings
              </RouterLink>
            </Button>
          </div>
        </div>
      </Card>

      <div id="events" class="space-y-6">
        <div class="flex items-center justify-between">
          <div>
            <h2 class="text-2xl font-bold tracking-tight">
              Upcoming Events
            </h2>
            <p class="text-sm text-muted-foreground">
              Live events available for seat reservation
            </p>
          </div>
        </div>

        <div v-if="isPending" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <Card v-for="n in 3" :key="n" class="overflow-hidden space-y-3 p-5">
            <Skeleton class="aspect-video w-full rounded-lg" />
            <Skeleton class="h-4 w-1/3" />
            <Skeleton class="h-6 w-3/4" />
            <Skeleton class="h-4 w-1/2" />
            <div class="pt-4 flex justify-between items-center">
              <Skeleton class="h-8 w-20" />
              <Skeleton class="h-8 w-28" />
            </div>
          </Card>
        </div>

        <div v-else-if="error">
          <Card v-if="!isAuthenticated()" class="p-8 text-center space-y-4 max-w-lg mx-auto">
            <div class="mx-auto flex h-12 w-12 items-center justify-center rounded-full bg-primary/10 text-primary">
              <LogIn class="h-6 w-6" />
            </div>
            <CardTitle>Sign in to view events</CardTitle>
            <CardDescription>
              Please log in to browse live events and reserve your seats in the ticket queue.
            </CardDescription>
            <Button as-child>
              <RouterLink to="/login">
                Sign In
              </RouterLink>
            </Button>
          </Card>
          <div
            v-else
            class="flex items-center justify-between rounded-lg border border-destructive/30 bg-destructive/10 p-4 text-sm text-destructive"
          >
            <div class="flex items-center gap-2">
              <AlertCircle class="h-5 w-5 shrink-0" />
              <span>{{ error.message }}</span>
            </div>
            <Button variant="outline" size="sm" @click="() => refetch()">
              Retry
            </Button>
          </div>
        </div>

        <div v-else-if="data?.data?.length" class="space-y-6">
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <Card
              v-for="event in data.data"
              :key="event.id"
              class="flex flex-col justify-between overflow-hidden transition-shadow hover:shadow-md"
            >
              <div>
                <div class="aspect-video w-full bg-muted flex items-center justify-center text-muted-foreground font-medium">
                  {{ event.name }}
                </div>
                <CardHeader class="pb-3">
                  <div class="flex items-center gap-2 text-xs text-muted-foreground">
                    <Calendar class="h-3.5 w-3.5" />
                    <span>{{ formatDate(event.startTime) }}</span>
                  </div>
                  <CardTitle class="text-xl">
                    {{ event.name }}
                  </CardTitle>
                  <CardDescription class="flex items-center gap-1.5 pt-1">
                    <Users class="h-3.5 w-3.5" />
                    <span>{{ event.venueCapacity }} Seats Capacity</span>
                  </CardDescription>
                </CardHeader>
                <CardContent>
                  <Badge variant="secondary">
                    Active
                  </Badge>
                </CardContent>
              </div>
              <CardFooter class="flex items-center justify-between border-t border-border pt-4">
                <span class="text-xs text-muted-foreground">#{{ event.id }}</span>
                <Button size="sm">
                  Join Queue & Book
                </Button>
              </CardFooter>
            </Card>
          </div>

          <div
            v-if="data?.meta && data.meta.totalPages > 1"
            class="flex items-center justify-between border-t border-border pt-4"
          >
            <p class="text-sm text-muted-foreground">
              Page {{ data.meta.page }} of {{ data.meta.totalPages }}
            </p>
            <div class="flex gap-2">
              <Button
                variant="outline"
                size="sm"
                :disabled="!data.meta.hasPreviousPage"
                @click="handlePageChange((query.page ?? 1) - 1)"
              >
                <ChevronLeft class="h-4 w-4 mr-1" />
                Previous
              </Button>
              <Button
                variant="outline"
                size="sm"
                :disabled="!data.meta.hasNextPage"
                @click="handlePageChange((query.page ?? 1) + 1)"
              >
                Next
                <ChevronRight class="h-4 w-4 ml-1" />
              </Button>
            </div>
          </div>
        </div>

        <Card v-else class="p-12 text-center text-muted-foreground">
          <p>No upcoming events available at the moment.</p>
        </Card>
      </div>
    </div>
  </AppLayout>
</template>
