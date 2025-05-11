

const setError = ($element, message) => {
    const $elementParent = $element.parent();
    const $errorDisplay = $elementParent.find('.error');

    // $elementParent.addClass('error').removeClass('success');
    $errorDisplay.text(message);
};

const setSuccess = ($element) => {
    const $elementParent = $element.parent();
    const $errorDisplay = $elementParent.find('.error');

    // $elementParent.addClass('success').removeClass('error');
    $errorDisplay.text('');
};

const isValidEmail = email => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
};

const validateForm = ($parentElement) => {

    let errorCount = 0;
    var index = $parentElement.data('index');

    const $candidateName = $parentElement.find('#candidate-name');
    const $genders = $parentElement.find(`input[name="Gender_${index}"]`);
    const $skills = $parentElement.find('input[name="skills"]');
    const $phoneNo = $parentElement.find('#phone');
    const $email = $parentElement.find('#email');
    const $country = $parentElement.find('#country');


    const isGenderSelected = $genders.is(':checked');
    const isChecked = $skills.is(':checked');
    console.log(isGenderSelected);


    if ($.trim($candidateName.val()) === '') {
        setError($candidateName, 'Candidate name is required')
        errorCount++;
    }
    else {
        setSuccess($candidateName);
    }
    if (!isGenderSelected) {
        setError($genders.first(), 'Please select gender.');
        errorCount++;
    }
    else {
        setSuccess($genders.first());
    }

    if (!isChecked) {
        setError($skills.first(), "Please select at least one skill.");
        errorCount++;

    }
    else {
        setSuccess($skills.first());
    }
    if ($.trim($phoneNo.val()) === '') {
        setError($phoneNo, "Please enter valid phone no.");
        errorCount++;
    }
    else {
        setSuccess($phoneNo);
    }


    if ($.trim($email.val()) === '' || !isValidEmail($.trim($email.val()))) {
        setError($email, "Please enter valid emil address.");
        errorCount++;
    }
    else {
        setSuccess($email);
    }
    console.log($country.val());

    if ($country.val() === '') {
        setError($country, "Please select country.");
        errorCount++;
    }
    else {
        setSuccess($country);
    }

    return errorCount == 0;


};

$(document).ready(() => {

    $('#btn-save').click(() => {

        console.log('btn save clicked');

        const $form = $('.register-form');
        const index = 0;

        if (!validateForm($form)) {
            return;
        }

        const skills = [];
        $form.find('input[name="skills"]:checked').each(function () {
            skills.push($(this).val());
        });

        const candidate = {
            candidateId: parseInt($('#ID').val()),
            name: $.trim($form.find('#candidate-name').val()),
            genderId: parseInt($form.find(`input[name="Gender_${index}"]:checked`)?.val()),
            skills: skills,
            skillset: skills.join(','),
            phone: $.trim($form.find('#phone').val()),
            email: $.trim($form.find('#email').val()),
            address: $.trim($form.find('textarea').val()),
            countryId: $.trim($form.find('#country').val())
        };

        console.log(candidate);

        console.log('save end');


        $.ajax({
            type: "POST",
            url: "/SaveCandidate",
            contentType: 'application/json',
            data: JSON.stringify(candidate),
            success: (response) => {
                console.log(response);
                if (response.success) {
                    window.location.href = '/Index';
                }
                else {
                    $('#error-tag').text(response.message);
                }
            },
            error: (err) => {
                Swal.fire({
                    title: "Error!",
                    text: error.responseText,
                    icon: "error",
                    confirmButtonColor: "#d33",
                    confirmButtonText: "OK"
                });
            }

        });

    });
});



