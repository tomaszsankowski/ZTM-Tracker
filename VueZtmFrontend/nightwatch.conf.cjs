const chromedriver = require('chromedriver');

module.exports = {
  src_folders: ['tests/e2e'],

  page_objects_path: [],
  custom_commands_path: [],
  custom_assertions_path: '',

  webdriver: {
    start_process: true,
    server_path: chromedriver.path,
    port: 9515
  },

  test_settings: {
    default: {
      disable_error_log: false,
      launch_url: 'http://localhost:5173',

      output_folder: false,

      screenshots: {
        enabled: true,
        path: 'screens',
        on_failure: true
      },

      desiredCapabilities: {
        browserName: 'chrome',
        'goog:chromeOptions': {
          w3c: true,
          args: [
            // '--headless'
          ]
        }
      }
    }
  }
};
