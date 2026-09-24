<script setup lang="ts">
import type { RegisterInput } from '@/lib/validations/auth'
import { AlertCircle, Loader2, Lock, Mail, User } from '@lucide/vue'
import { reactive } from 'vue'
import { RouterLink } from 'vue-router'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { useRegisterMutation } from '@/lib/api/auth-mutations'
import { registerSchema } from '@/lib/validations/auth'

const form = reactive<RegisterInput>({
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
})

const errors = reactive<Record<string, string>>({})
const { mutate, isPending, error } = useRegisterMutation()

function handleSubmit() {
  Object.keys(errors).forEach(key => delete errors[key])

  const validation = registerSchema.safeParse(form)

  if (!validation.success) {
    validation.error.issues.forEach((issue) => {
      const field = issue.path[0] as string
      if (field && !errors[field]) {
        errors[field] = issue.message
      }
    })
    return
  }

  mutate(validation.data)
}
</script>

<template>
  <Card class="w-full max-w-md mx-auto shadow-lg">
    <CardHeader class="space-y-1">
      <CardTitle class="text-2xl font-bold tracking-tight text-center">
        Create an account
      </CardTitle>
      <CardDescription class="text-center">
        Enter your details below to create your ticket account
      </CardDescription>
    </CardHeader>

    <form @submit.prevent="handleSubmit">
      <CardContent class="space-y-4">
        <div
          v-if="error"
          class="flex items-center gap-2 rounded-lg border border-destructive/30 bg-destructive/10 p-3 text-sm text-destructive"
        >
          <AlertCircle class="h-4 w-4 shrink-0" />
          <span>{{ error.message }}</span>
        </div>

        <div class="space-y-2">
          <Label for="username">Username</Label>
          <div class="relative">
            <User class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
            <Input
              id="username"
              v-model="form.username"
              type="text"
              placeholder="johndoe"
              class="pl-9"
              :class="{ 'border-destructive focus-visible:ring-destructive': errors.username }"
            />
          </div>
          <p v-if="errors.username" class="text-xs font-medium text-destructive">
            {{ errors.username }}
          </p>
        </div>

        <div class="space-y-2">
          <Label for="email">Email</Label>
          <div class="relative">
            <Mail class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
            <Input
              id="email"
              v-model="form.email"
              type="email"
              placeholder="name@example.com"
              class="pl-9"
              :class="{ 'border-destructive focus-visible:ring-destructive': errors.email }"
            />
          </div>
          <p v-if="errors.email" class="text-xs font-medium text-destructive">
            {{ errors.email }}
          </p>
        </div>

        <div class="space-y-2">
          <Label for="password">Password</Label>
          <div class="relative">
            <Lock class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
            <Input
              id="password"
              v-model="form.password"
              type="password"
              placeholder="••••••••"
              class="pl-9"
              :class="{ 'border-destructive focus-visible:ring-destructive': errors.password }"
            />
          </div>
          <p v-if="errors.password" class="text-xs font-medium text-destructive">
            {{ errors.password }}
          </p>
        </div>

        <div class="space-y-2">
          <Label for="confirmPassword">Confirm Password</Label>
          <div class="relative">
            <Lock class="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
            <Input
              id="confirmPassword"
              v-model="form.confirmPassword"
              type="password"
              placeholder="••••••••"
              class="pl-9"
              :class="{ 'border-destructive focus-visible:ring-destructive': errors.confirmPassword }"
            />
          </div>
          <p v-if="errors.confirmPassword" class="text-xs font-medium text-destructive">
            {{ errors.confirmPassword }}
          </p>
        </div>
      </CardContent>

      <CardFooter class="flex flex-col space-y-4 pt-2">
        <Button type="submit" class="w-full" :disabled="isPending">
          <Loader2 v-if="isPending" class="mr-2 h-4 w-4 animate-spin" />
          <span>{{ isPending ? 'Creating account...' : 'Create Account' }}</span>
        </Button>

        <p class="text-sm text-center text-muted-foreground">
          Already have an account?
          <RouterLink to="/login" class="font-medium text-primary hover:underline">
            Sign in
          </RouterLink>
        </p>
      </CardFooter>
    </form>
  </Card>
</template>
