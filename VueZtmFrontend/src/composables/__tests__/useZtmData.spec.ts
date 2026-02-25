import { describe, it, expect, vi } from 'vitest'
import { useZtmData } from '@/composables/useZtmData'
import apiClient from '@/api/axios'

vi.mock('@/api/axios', () => ({
  default: {
    get: vi.fn()
  }
}))

describe('useZtmData', () => {
  it('fetches delays successfully', async () => {
    const mockData = { delay: [] }
    ;(apiClient.get as any).mockResolvedValue({ data: mockData })

    const { data, fetchDelays } = useZtmData()
    await fetchDelays(123)

    expect(data.value).toEqual(mockData)
    expect(apiClient.get).toHaveBeenCalledWith('/Ztm/delays/123')
  })
})
