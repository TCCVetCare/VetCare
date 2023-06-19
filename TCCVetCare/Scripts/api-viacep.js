async function searchAddressByZipCode() {

    const zipCodeInput = document.getElementById('zip-code');
    const zipCode = zipCodeInput.value;

    try {

        const response = await fetch(`https://viacep.com.br/ws/${zipCode}/json/`);
        const data = await response.json();

        const streetNameInput = document.getElementById('street-name');
        const neighborhoodInput = document.getElementById('neighborhood');
        const cityInput = document.getElementById('city');
        const stateInput = document.getElementById('state');

        streetNameInput.value = data.logradouro;
        neighborhoodInput.value = data.bairro;
        cityInput.value = data.localidade;
        stateInput.value = data.uf;

    } catch (error) {

        console.log('Falha ao consultar o CEP:', error);

    }

}