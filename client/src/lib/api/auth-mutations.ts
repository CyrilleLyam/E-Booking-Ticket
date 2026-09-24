import type { LoginInput, RegisterInput } from '@/lib/validations/auth'
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import { useRouter } from 'vue-router'
import { loginUser, registerUser } from '@/lib/api/auth'
import { setAuthTokens } from '@/lib/cookies'

export function useLoginMutation() {
  const router = useRouter()
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (data: LoginInput) => loginUser(data),
    onSuccess: (response) => {
      setAuthTokens(response.data)
      queryClient.invalidateQueries({ queryKey: ['currentUser'] })
      router.push('/')
    },
  })
}

export function useRegisterMutation() {
  const router = useRouter()
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (data: RegisterInput) => registerUser(data),
    onSuccess: (response) => {
      setAuthTokens(response.data)
      queryClient.invalidateQueries({ queryKey: ['currentUser'] })
      router.push('/')
    },
  })
}
