export interface EventItem {
  id: number
  name: string
  startTime: string
  venueCapacity: number
  createdAt: string
  updatedAt: string
}

export interface EventQuery {
  page?: number
  pageSize?: number
  name?: string
  startTime?: string
}
