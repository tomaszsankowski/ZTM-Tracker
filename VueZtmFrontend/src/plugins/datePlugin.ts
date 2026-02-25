export default {
  install: (app: any) => {
    app.config.globalProperties.$formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleString();
    };
  }
};
