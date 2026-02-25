import { describe, it, expect, vi } from 'vitest';
import { mount } from '@vue/test-utils';
import NotificationToast from '../NotificationToast.vue';
import { createTestingPinia } from '@pinia/testing';
import { setActivePinia } from 'pinia';

describe('NotificationToast', () => {
  it('renders when show is true', async () => {
    const pinia = createTestingPinia({
      createSpy: vi.fn,
      initialState: {
        notification: { show: true, message: 'Test Message', type: 'success' }
      }
    });
    setActivePinia(pinia);

    const wrapper = mount(NotificationToast, {
      global: {
        plugins: [pinia],
      },
    });

    expect(wrapper.text()).toContain('Test Message');
    expect(wrapper.text()).toContain('Success');
  });

  it('does not render when show is false', async () => {
    const pinia = createTestingPinia({
      createSpy: vi.fn,
      initialState: {
        notification: { show: false, message: 'Test Message', type: 'success' }
      }
    });
    setActivePinia(pinia);

    const wrapper = mount(NotificationToast, {
      global: {
        plugins: [pinia],
      },
    });

    expect(wrapper.find('.fixed').exists()).toBe(false);
  });
});
