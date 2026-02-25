import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import DelaysTable from '@/components/DelaysTable.vue'

describe('DelaysTable', () => {
  it('renders delays correctly', () => {
    const delays = {
      departures: [
        { id: '1', routeId: '10', headsign: 'Test', estimatedTime: '2024-05-20T12:00:00', delayInSeconds: 0 }
      ]
    }
    const wrapper = mount(DelaysTable, {
      props: { delays, stopId: 123 },
      global: {
        mocks: {
          $formatDate: (date: string) => date
        },
        directives: {
          'delay-color': () => {}
        }
      }
    })

    expect(wrapper.text()).toContain('Live Delays for Stop 123')
    expect(wrapper.text()).toContain('Test')
  })
})
