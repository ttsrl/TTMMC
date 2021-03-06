﻿$(document).ready(function () {

    jQuery.validator.addMethod("notEqual", function (value, element, param) {
            return this.optional(element) || (value !== param && value !== $(param).val());
    }, "Please specify a different (non-default) value");

    jQuery.validator.addMethod("passwordCheck", function (value, element, param) {
        if (this.optional(element)) {
            return true;
        } else if (!/[A-Z]/.test(value)) {
            return false;
        } else if (!/[a-z]/.test(value)) {
            return false;
        } else if (!/[0-9]/.test(value)) {
            return false;
        }
        return true;
    }, "");

    jQuery.validator.addMethod("populated", function (elmSelected, element, param) {
        if (param === true) {
            if (element.childElementCount > 0) {
                return true;
            }
        }
        return false;
    }, "");

    jQuery.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || element.files[0].size <= param;
    }, "");

    jQuery.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
    }, "");

});

var validateNewMouldRules = {
    'code': { required: true },
    'description': { required: true },
    'location': { required: true },
    'image': { extension: "jpg|jpeg|png", filesize: 3145728 }
};
var validateNewMouldMessages = {
    'code': { required: "Inserisci un codice" },
    'description': { required: "Inserisci una descrizione" },
    'location': { required: "Inserisci la posizione dello stampo" },
    'image': { extension:"Estensione file non supportata", filesize: "Dimensione file superiore a 3MB" }
};

var validateNewClientRules = {
    'name': { required: true },
    'vat': { required: true, digits: true },
    'fiscalCode': { required: true },
    'state': { required: true },
    'province': { required: true },
    'town': { required: true },
    'address': { required: true },
    'addressNumber': { required: true },
    'phone': { digits: true },
    'email': { email: true },
    'pec': { email: true }
};

var validateNewClientMessages = {
    'name': { required: "Inserisci un nome" },
    'vat': { required: "Inserisci una partita i.v.a", digits: "Partita i.v.a non valida" },
    'fiscalCode': { required: "Inserisci un codice fiscale" },
    'state': { required: "Inserisci uno stato" },
    'province': { required: "Inserisci una provincia" },
    'town': { required: "Inserisci un comune" },
    'address': { required: "Inserisci un'indirizzo" },
    'addressNumber': { required: "Inserisci un numero civico" },
    'phone': { digits: "Numero di telefono non valido" },
    'email': { email: "Email non valida" },
    'pec': { email: "Pec non valida" }
};

var validateEditClientRules = {
    'Name': { required: true },
    'vat': { required: true, digits: true },
    'FiscalCode': { required: true },
    'State': { required: true },
    'Province': { required: true },
    'Town': { required: true },
    'addressStreetMode': { required: true },
    'addressStreet': { required: true },
    'addressNumber': { required: true },
    'Address': { required: true },
    'Phone': { digits: true },
    'Email': { email: true },
    'pec': { email: true }
};

var validateEditClientMessages = {
    'Name': { required: "Inserisci un nome" },
    'vat': { required: "Inserisci una partita i.v.a", digits: "Partita i.v.a non valida" },
    'FiscalCode': { required: "Inserisci un codice fiscale" },
    'State': { required: "Inserisci uno stato" },
    'Province': { required: "Inserisci una provincia" },
    'Town': { required: "Inserisci un comune" },
    'addressStreetMode': { required: "" },
    'addressStreet': { required: "Inserisci un'indirizzo" },
    'addressNumber': { required: "Inserisci un numero civico" },
    'Address': { required: "" },
    'Phone': { digits: "Numero di telefono non valido" },
    'Email': { email: "Email non valida" },
    'pec': { email: "Pec non valida" }
};

var validateMaterialRules = {
    'name': { required: true }
};

var validateMaterialMessages = {
    'name': { required: "" }
};

var validateNewMixtureRules = {
    'name': { required: true, maxlength: 22 },
    'components': { populated: true }
};

var validateNewMasterRules = {
    'code': { required: true },
    'name': { required: true },
    'hexColor': { required: true }
};

var validateNewLayoutRules = {
    'client': { required: true },
    'mould': { required: true },
    'mixture': { required: true },
    'machine': { required: true },
    'quantity': { required: true },
    'start': { required: true }
};

var validateNewLayoutMessages = {
    'client': { required: "Inserire un cliente" },
    'mould': { required: "Inserire uno stampo" },
    'mixture': { required: "Inserire una miscela" },
    'machine': { required: "Inserire una macchina" },
    'quantity': { required: "Inserire una quantità" },
    'start': { required: "Inserire una data" }
};