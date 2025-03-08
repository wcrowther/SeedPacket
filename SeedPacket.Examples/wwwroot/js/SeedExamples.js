
(function (seedExamples, $, undefined)
{
	seedExamples.alert = (msg) => alert(msg); 

	seedExamples.init = function (){

		$('#numberOfRows, #randomSeed').on('change',
			() => $('#rowResults').load(`/Data/GetResultRows/
											?rows=${ $('#numberOfRows').val() }
											&seed=${ $('#randomSeed').val() || 1234 }`)
        )

		$('#advNumberOfRows, #advRandomSeed').on('change',
			() => $('#advRowResults').load(`/Data/GetAdvancedResultRows/
												?rows=${ $('#advNumberOfRows').val() }
												&seed=${ $('#advRandomSeed').val() || 1234 }`)
		)
	}

})(window.seedExamples = window.seedExamples || {}, jQuery);


