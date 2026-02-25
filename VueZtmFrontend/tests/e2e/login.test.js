describe('Login E2E Tests', function() {
  
  before((browser) => {
  });

  after((browser) => {
    browser.end();
  });

  it('Sprawdzenie logowania', function(browser) {
    browser
      .url('http://localhost:5173/login')
      .waitForElementVisible('body', 5000)
      .assert.visible('input#login', 'Pole login jest widoczne')
      .assert.visible('input#password', 'Pole hasło jest widoczne')
      .assert.visible('button[type=submit]', 'Przycisk logowania jest widoczny')
      
      .setValue('input#login', 'admin')
      .setValue('input#password', 'tajnehaslo')
      
      .click('button[type=submit]')
      
      .pause(1000);
  });
});