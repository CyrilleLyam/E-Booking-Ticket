import type { Ref } from 'vue'
import type { EventQuery } from '@/types'
import { useQuery } from '@tanstack/vue-query'
import { getEventById, getEvents } from '@/lib/api/events'

export function useEventsQuery(query?: Ref<EventQuery> | EventQuery) {
  return useQuery({
    queryKey: ['events', query],
    queryFn: () => getEvents(query && 'value' in query ? query.value : query),
  })
}

export function useEventDetailQuery(id: Ref<number> | number) {
  return useQuery({
    queryKey: ['event', id],
    queryFn: () => getEventById(typeof id === 'object' && 'value' in id ? id.value : id),
  })
}
