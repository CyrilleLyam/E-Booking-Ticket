import type { BaseApiResponse, EventItem, EventQuery } from '@/types'
import { apiClient } from '@/lib/api/client'

export async function getEvents(query?: EventQuery): Promise<BaseApiResponse<EventItem[]>> {
  const response = await apiClient.get<BaseApiResponse<EventItem[]>>('/events', {
    params: query,
  })
  return response.data
}

export async function getEventById(id: number): Promise<BaseApiResponse<EventItem>> {
  const response = await apiClient.get<BaseApiResponse<EventItem>>(`/events/${id}`)
  return response.data
}
