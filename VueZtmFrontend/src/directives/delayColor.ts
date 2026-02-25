export const delayColor = {
  mounted(el: HTMLElement, binding: any) {
    const delay = binding.value;
    if (delay > 180) {
      el.style.color = 'red';
      el.style.fontWeight = 'bold';
    } else if (delay > 60) {
      el.style.color = 'orange';
    } else {
      el.style.color = 'green';
    }
  },
  updated(el: HTMLElement, binding: any) {
    const delay = binding.value;
    if (delay > 180) {
      el.style.color = 'red';
      el.style.fontWeight = 'bold';
    } else if (delay > 60) {
      el.style.color = 'orange';
    } else {
      el.style.color = 'green';
    }
  }
};
